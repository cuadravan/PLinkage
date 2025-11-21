using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using MongoDB.Driver;
using System.Reflection;

namespace PLinkageAPI.Services
{
    public class OfferApplicationService: IOfferApplicationService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepository;
        private readonly IRepository<ProjectOwner> _projectOwnerRepository;
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<OfferApplication> _offerApplicationRepository;
        private readonly IMongoClient _mongoClient;

        public OfferApplicationService(IMongoClient mongoClient, IRepository<SkillProvider> skillProviderRepository, IRepository<ProjectOwner> projectOwnerRepository, IRepository<Admin> adminRepository, IRepository<Project> projectRepository, IRepository<OfferApplication> offerApplicationRepository)
        {
            _skillProviderRepository = skillProviderRepository;
            _projectOwnerRepository = projectOwnerRepository;
            _adminRepository = adminRepository;
            _projectRepository = projectRepository;
            _offerApplicationRepository = offerApplicationRepository;
            _mongoClient = mongoClient;
        }
        public async Task<ApiResponse<Guid>> CreateApplicationOffer(OfferApplicationCreationDto offerApplicationCreationDto)
        {
            UserRole userRole = offerApplicationCreationDto.UserRoleOfCreator;
            IUser? sender;
            IUser? receiver;
            if(userRole == UserRole.SkillProvider)
            {
                sender = await _skillProviderRepository.GetByIdAsync(offerApplicationCreationDto.SenderId);
                receiver = await _projectOwnerRepository.GetByIdAsync(offerApplicationCreationDto.ReceiverId);
            }
            else if (userRole == UserRole.ProjectOwner)
            {
                sender = await _projectOwnerRepository.GetByIdAsync(offerApplicationCreationDto.SenderId);
                receiver = await _skillProviderRepository.GetByIdAsync(offerApplicationCreationDto.ReceiverId);
            }
            else
            {
                return ApiResponse<Guid>.Fail("User Role cannot make offer/application.");
            }


            if (sender == null || receiver == null)
            {
                return ApiResponse<Guid>.Fail("Sender/receiver do not exist in database.");
            }

            var newId = Guid.NewGuid();
            var newOfferApplication = new OfferApplication
            {
                OfferApplicationId = newId,
                OfferApplicationType = offerApplicationCreationDto.OfferApplicationType,
                SenderId = offerApplicationCreationDto.SenderId,
                ReceiverId = offerApplicationCreationDto.ReceiverId,
                ProjectId = offerApplicationCreationDto.ProjectId,
                OfferApplicationStatus = "Pending",
                OfferApplicationRate = offerApplicationCreationDto.OfferApplicationRate,
                OfferApplicationTimeFrame = offerApplicationCreationDto.OfferApplicationTimeFrame
            };

            sender.AddOfferApplication(newId);
            receiver.AddOfferApplication(newId);

            using (var session = await _mongoClient.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    // This call will use the (session, entity) overload
                    await _offerApplicationRepository.AddAsync(newOfferApplication, session);

                    // These calls will also use the (session, entity) overload
                    if (sender is SkillProvider skillProviderSender)
                    {
                        await _skillProviderRepository.UpdateAsync(skillProviderSender);
                    }
                    else if (sender is ProjectOwner projectOwnerSender)
                    {
                        await _projectOwnerRepository.UpdateAsync(projectOwnerSender);
                    }

                    if (receiver is SkillProvider skillProviderReceiver)
                    {
                        await _skillProviderRepository.UpdateAsync(skillProviderReceiver);
                    }
                    else if (receiver is ProjectOwner projectOwnerReceiver)
                    {
                        await _projectOwnerRepository.UpdateAsync(projectOwnerReceiver);
                    }

                    await session.CommitTransactionAsync();
                    return ApiResponse<Guid>.Ok(newOfferApplication.OfferApplicationId);
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    return ApiResponse<Guid>.Fail($"An error occurred: {ex.Message}");
                }
            }
        }
        public async Task<ApiResponse<OfferApplication>> GetSpecificOfferApplication(Guid offerApplicationId)
        {
            var offerApplication = await _offerApplicationRepository.GetByIdAsync(offerApplicationId);
            if (offerApplication == null)
                return ApiResponse<OfferApplication>.Fail($"OfferApplication with ID {offerApplicationId} not found.");

            return ApiResponse<OfferApplication>.Ok(offerApplication);
        }
        public async Task<ApiResponse<OfferApplicationPageDto>> GetOfferApplicationOfUser(Guid userId, UserRole userRole)
        {
            IUser? currentUser;
            if (userRole == UserRole.SkillProvider)
            {
                currentUser = await _skillProviderRepository.GetByIdAsync(userId);
            }
            else if (userRole == UserRole.ProjectOwner)
            {
                currentUser = await _projectOwnerRepository.GetByIdAsync(userId);
            }
            else
            {
                return ApiResponse<OfferApplicationPageDto>.Fail("User Role does not have offer/application.");
            }

            if (currentUser == null)
            {
                return ApiResponse<OfferApplicationPageDto>.Fail("Sender/receiver do not exist in database.");
            }

            var offerApplicationList = await _offerApplicationRepository.GetByIdsAsync(currentUser.OfferApplicationId);

            if (offerApplicationList == null || !offerApplicationList.Any())
            {
                return ApiResponse<OfferApplicationPageDto>.Ok(new OfferApplicationPageDto());
            }

            var projectIds = new HashSet<Guid>();
            var userIds = new HashSet<Guid>();

            foreach (var item in offerApplicationList)
            {
                projectIds.Add(item.ProjectId);
                userIds.Add(item.SenderId);
                userIds.Add(item.ReceiverId);
            }

            var projectsTask = _projectRepository.GetByIdsAsync(projectIds);
            var skillProvidersTask = _skillProviderRepository.GetByIdsAsync(userIds);
            var projectOwnersTask = _projectOwnerRepository.GetByIdsAsync(userIds);

            await Task.WhenAll(projectsTask, skillProvidersTask, projectOwnersTask);

            var projects = (await projectsTask).ToDictionary(p => p.ProjectId);

            var users = new Dictionary<Guid, IUser>();
            foreach (var sp in await skillProvidersTask)
            {
                if (sp != null)
                    users[sp.UserId] = (IUser)sp;
            }
            foreach (var po in await projectOwnersTask)
            {
                if (po != null)
                    users[po.UserId] = (IUser)po;
            }

            var pendingSent = new List<OfferApplicationDisplayDto>();
            var pendingReceived = new List<OfferApplicationDisplayDto>();
            var historySent = new List<OfferApplicationDisplayDto>();
            var historyReceived = new List<OfferApplicationDisplayDto>();

            foreach (var offerApplicationItem in offerApplicationList)
            {
                if (!projects.TryGetValue(offerApplicationItem.ProjectId, out var project) ||
                    !users.TryGetValue(offerApplicationItem.SenderId, out var sender) ||
                    !users.TryGetValue(offerApplicationItem.ReceiverId, out var receiver))
                {
                    continue;
                }

                var dto = CreateDisplayDto(offerApplicationItem, project, sender, receiver, userId);

                bool isSender = offerApplicationItem.SenderId == userId;
                bool isPendingOrNegotiating = offerApplicationItem.OfferApplicationStatus == "Pending";

                if (isSender && isPendingOrNegotiating)
                    pendingSent.Add(dto);
                else if (isSender && !isPendingOrNegotiating)
                    historySent.Add(dto);
                else if (!isSender && isPendingOrNegotiating)
                    pendingReceived.Add(dto);
                else
                    historyReceived.Add(dto);
            }

            var newOfferApplicationPageDto = new OfferApplicationPageDto
            {
                ReceivedPending = pendingReceived,
                ReceivedHistory = historyReceived,
                SentPending = pendingSent,
                SentHistory = historySent
            };

            return ApiResponse<OfferApplicationPageDto>.Ok(newOfferApplicationPageDto);
        }

        private OfferApplicationDisplayDto CreateDisplayDto(OfferApplication item, Project project, IUser sender, IUser receiver, Guid userId)
        {
            // We determine the name to display in the linkage and the ID to link to
            string formattedConcernedName = string.Empty;
            Guid concernedPersonId = Guid.Empty;
            if (userId == receiver.UserId)
            {
                formattedConcernedName = $"Sent by: " + sender.UserFirstName + " " + sender.UserLastName;
                concernedPersonId = sender.UserId;
            }
            else
            {
                formattedConcernedName = $"Received by: " + receiver.UserFirstName + " " + receiver.UserLastName;
                concernedPersonId = receiver.UserId;
            }
            // We determine the format and rate to be displayed (which varies if its negotiated or not)
            string formattedRate = string.Empty;
            string formattedTimeFrame = string.Empty;
            if (item.IsNegotiating)
            {
                formattedRate = $"₱{item.OfferApplicationRate:N2} per hour (prev. ₱{item.OldOfferApplicationRate:N2} per hour)";
                formattedTimeFrame = $"{item.OfferApplicationTimeFrame:N0} hrs (prev. {item.OldOfferApplicationTimeFrame:N0} hrs)";
            }
            else
            {
                formattedRate = $"₱{item.OfferApplicationRate:N2} per hour";
                formattedTimeFrame = $"{item.OfferApplicationTimeFrame:N0} hrs";
            }
            bool awaitingResponse = false; // Accept or Reject
            bool isNegotiable = false;      
            if (item.OfferApplicationStatus == "Pending" && userId == receiver.UserId && item.IsNegotiating == false)
            { // If user is receiver of a pending linkage, and it is not negotiating
                awaitingResponse = true; // They can respond
            }
            if (item.OfferApplicationStatus == "Pending" && userId == receiver.UserId && item.IsNegotiating == false && receiver.UserRole == UserRole.SkillProvider)
            { // If user is receiver of a pending linkage, and it is not negotiating, and user is a skill provider
                isNegotiable = true; // They can respond and additionally negotiate
            }
            if (item.OfferApplicationStatus == "Pending" && userId == sender.UserId && item.IsNegotiating)
            { // If user is the sender of a pending linkage and it is in negotiating mode
                awaitingResponse = true;
            }

            return new OfferApplicationDisplayDto
            {
                OfferApplicationId = item.OfferApplicationId,
                ProjectName = project.ProjectName,
                FormattedConcernedName = formattedConcernedName,
                ConcernedId = concernedPersonId,
                OfferApplicationType = item.OfferApplicationType,
                OfferApplicationStatus = item.OfferApplicationStatus,
                FormattedRate = formattedRate,
                FormattedTimeFrame = formattedTimeFrame,
                AwaitingResponse = awaitingResponse,
                IsNegotiating = item.IsNegotiating,
                IsNegotiable = isNegotiable,
                SenderId = item.SenderId,
                ReceiverId = item.ReceiverId,
                ProjectId = item.ProjectId
            };
        }
        public async Task<ApiResponse<bool>> ProcessOfferApplication(OfferApplicationProcessDto offerApplicationProcessDto)
        {
            if(offerApplicationProcessDto.Process == "Approve")
            {
                Task<SkillProvider?> skillProviderTask = Task.FromResult<SkillProvider?>(null);
                if (offerApplicationProcessDto.Type == "Application")
                {
                    skillProviderTask = _skillProviderRepository.GetByIdAsync(offerApplicationProcessDto.SenderId);
                }
                else
                {
                    skillProviderTask = _skillProviderRepository.GetByIdAsync(offerApplicationProcessDto.ReceiverId);
                }
                var offerApplicationTask = _offerApplicationRepository.GetByIdAsync(offerApplicationProcessDto.OfferApplicationId);
                var projectTask = _projectRepository.GetByIdAsync(offerApplicationProcessDto.ProjectId);
                await Task.WhenAll(offerApplicationTask, projectTask, skillProviderTask);

                var offerApplication = offerApplicationTask.Result;
                var project = projectTask.Result;
                var skillProvider = skillProviderTask.Result;
                if(offerApplication == null || project == null || skillProvider == null)
                {
                    return ApiResponse<bool>.Fail("Your data seems to be out of sync in the server. Contact an administrator.");
                }
                else if (project.ProjectStatus == ProjectStatus.Active && project.ProjectResourcesAvailable > 0 && !project.ProjectMembers.Any(pm => pm.MemberId == skillProvider.UserId))
                {
                    offerApplication.OfferApplicationStatus = "Accepted";
                    offerApplication.IsNegotiating = false;
                    project.EmployMember(skillProvider, offerApplication.OfferApplicationTimeFrame, offerApplication.OfferApplicationRate);
                    skillProvider.AddProject(project.ProjectId);
                }
                else
                {
                    return ApiResponse<bool>.Fail("Cannot employ user if project is not active, already full, or user is already employed in the project.");
                }
                using (var session = await _mongoClient.StartSessionAsync())
                {
                    session.StartTransaction();
                    try
                    {
                        await _offerApplicationRepository.UpdateAsync(offerApplication, session);
                        await _projectRepository.UpdateAsync(project, session);
                        await _skillProviderRepository.UpdateAsync(skillProvider, session);

                        await session.CommitTransactionAsync();
                        return ApiResponse<bool>.Ok(true);
                    }
                    catch (Exception ex)
                    {
                        await session.AbortTransactionAsync();
                        return ApiResponse<bool>.Fail("There was an error in saving the changes. Please try again.");
                    }
                }

            }
            else if (offerApplicationProcessDto.Process == "Reject")
            {
                var offerApplication = await _offerApplicationRepository.GetByIdAsync(offerApplicationProcessDto.OfferApplicationId);
                if(offerApplication == null)
                    return ApiResponse<bool>.Fail("Your data seems to be out of sync in the server. Contact an administrator.");
                if(offerApplication.IsNegotiating)
                {
                    offerApplication.IsNegotiating = false;
                    offerApplication.OfferApplicationRate = offerApplication.OldOfferApplicationRate;
                    offerApplication.OfferApplicationTimeFrame = offerApplication.OldOfferApplicationTimeFrame;
                }
                else
                {
                    offerApplication.OfferApplicationStatus = "Rejected";
                }
                await _offerApplicationRepository.UpdateAsync(offerApplication);
                return ApiResponse<bool>.Ok(true);
            }
            else //Negotiate
            {
                var offerApplication = await _offerApplicationRepository.GetByIdAsync(offerApplicationProcessDto.OfferApplicationId);
                if (offerApplication == null)
                    return ApiResponse<bool>.Fail("Your data seems to be out of sync in the server. Contact an administrator.");

                if(offerApplication.OfferApplicationType == "Application")
                    return ApiResponse<bool>.Fail("Cannot negotiate an application.");

                if (offerApplication.NegotiationCount < 3)
                {
                    offerApplication.OldOfferApplicationRate = offerApplication.OfferApplicationRate;
                    offerApplication.OfferApplicationRate = offerApplicationProcessDto.NegotiatedRate;
                    offerApplication.OldOfferApplicationTimeFrame = offerApplication.OfferApplicationTimeFrame;
                    offerApplication.OfferApplicationTimeFrame = offerApplicationProcessDto.NegotiatedTimeFrame;
                    offerApplication.NegotiationCount += 1;
                    offerApplication.IsNegotiating = true;
                }
                else
                {
                    return ApiResponse<bool>.Fail("Negotiating limit has been reached.");
                }

                    await _offerApplicationRepository.UpdateAsync(offerApplication);
                return ApiResponse<bool>.Ok(true);
            }
        }
    }
}
