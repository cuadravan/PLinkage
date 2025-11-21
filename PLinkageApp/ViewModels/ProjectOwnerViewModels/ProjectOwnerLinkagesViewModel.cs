using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLinkageApp.Interfaces;
using PLinkageShared.DTOs;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PLinkageApp.ViewModels
{
    [QueryProperty(nameof(ForceReset), "ForceReset")]
    public partial class ProjectOwnerLinkagesViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IOfferApplicationServiceClient _offerApplicationServiceClient;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private OfferApplicationPageDto _allData;
        private bool _isInitialized = false;

        [ObservableProperty]
        private string selectedTopToggle = "Sent";
        [ObservableProperty]
        private string selectedBottomToggle = "Pending";
        [ObservableProperty]
        private bool isBusy = false;

        // ---------------------------------------------------------
        // PROPERTY FOR ANDROID (Dynamic Single List)
        // ---------------------------------------------------------
        [ObservableProperty]
        private ObservableCollection<OfferApplicationDisplayDto> offerApplicationCards = new ObservableCollection<OfferApplicationDisplayDto>();

        // ---------------------------------------------------------
        // PROPERTIES FOR WINDOWS (Static Separate Lists)
        // Added these so the Windows Grid can bind to them directly
        // ---------------------------------------------------------
        public ObservableCollection<OfferApplicationDisplayDto> SentOffersPending { get; } = new();
        public ObservableCollection<OfferApplicationDisplayDto> SentOffersHistory { get; } = new();
        public ObservableCollection<OfferApplicationDisplayDto> ReceivedApplicationsPending { get; } = new();
        public ObservableCollection<OfferApplicationDisplayDto> ReceivedApplicationsHistory { get; } = new();

        public bool ForceReset { get; set; } = false;

        public ProjectOwnerLinkagesViewModel(IDialogService dialogService, INavigationService navigationService, ISessionService sessionService, IOfferApplicationServiceClient offerApplicationServiceClient)
        {
            _sessionService = sessionService;
            _offerApplicationServiceClient = offerApplicationServiceClient;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized && !ForceReset)
                return;
            ForceReset = false;
            await LoadAllData();
        }

        [RelayCommand]
        private async Task Refresh()
        {
            _allData = null;
            await LoadAllData();
        }

        [RelayCommand]
        private async Task UpdateTopToggle(string selection)
        {
            if (IsBusy) return;
            if (string.IsNullOrEmpty(selection) || SelectedTopToggle == selection) return;

            SelectedTopToggle = selection;
            await UpdateDisplayedCards();
        }

        [RelayCommand]
        private async Task UpdateBottomToggle(string selection)
        {
            if (IsBusy) return;
            if (string.IsNullOrEmpty(selection) || SelectedBottomToggle == selection) return;

            SelectedBottomToggle = selection;
            await UpdateDisplayedCards();
        }

        [RelayCommand]
        private async Task Accept(OfferApplicationDisplayDto dto)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                string rawRate = Regex.Match(dto.FormattedRate, @"\d+(\.\d+)?").Value;
                string rawTimeFrame = Regex.Match(dto.FormattedTimeFrame, @"\d+(\.\d+)?").Value;

                var processDto = new OfferApplicationProcessDto
                {
                    OfferApplicationId = dto.OfferApplicationId,
                    Process = "Approve",
                    Type = dto.OfferApplicationType,
                    SenderId = dto.SenderId,
                    ReceiverId = dto.ReceiverId,
                    ProjectId = dto.ProjectId,
                    NegotiatedRate = decimal.Parse(rawRate),
                    NegotiatedTimeFrame = int.Parse(rawTimeFrame)
                };

                var result = await _offerApplicationServiceClient.ProcessOfferApplication(processDto);

                if (result.Success)
                {
                    await _dialogService.ShowAlertAsync("Success", "You have successfully approved this request.", "Ok");
                    ForceReset = true;
                    // Trigger reload to update UI on both platforms
                    await Refresh();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Error", $"There was an error in processing your request. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"There was an error in processing your request. Error: {ex}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Reject(OfferApplicationDisplayDto dto)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                string rawRate = Regex.Match(dto.FormattedRate, @"\d+(\.\d+)?").Value;
                string rawTimeFrame = Regex.Match(dto.FormattedTimeFrame, @"\d+(\.\d+)?").Value;

                var processDto = new OfferApplicationProcessDto
                {
                    OfferApplicationId = dto.OfferApplicationId,
                    Process = "Reject",
                    Type = dto.OfferApplicationType,
                    SenderId = dto.SenderId,
                    ReceiverId = dto.ReceiverId,
                    ProjectId = dto.ProjectId,
                    NegotiatedRate = decimal.Parse(rawRate),
                    NegotiatedTimeFrame = int.Parse(rawTimeFrame)
                };

                var result = await _offerApplicationServiceClient.ProcessOfferApplication(processDto);

                if (result.Success)
                {
                    await _dialogService.ShowAlertAsync("Success", "You have successfully rejected this request.", "Ok");
                    ForceReset = true;
                    // Trigger reload to update UI on both platforms
                    await Refresh();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Error", $"There was an error in processing your request. Server sent the following message: {result.Message}", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Error", $"There was an error in processing your request. Error: {ex}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ViewProject(OfferApplicationDisplayDto dto)
        {
            if (IsBusy)
                return;
            await _navigationService.NavigateToAsync("ViewProjectView", new Dictionary<string, object> { { "ProjectId", dto.ProjectId } });
        }

        [RelayCommand]
        private async Task ViewConcerned(OfferApplicationDisplayDto dto)
        {
            if (IsBusy)
                return;
            if(dto.ConcernedUserRole is PLinkageShared.Enums.UserRole.ProjectOwner)
                await _navigationService.NavigateToAsync("ViewProjectOwnerProfileView", new Dictionary<string, object> { { "ProjectOwnerId", dto.ConcernedId } });
            else if(dto.ConcernedUserRole is PLinkageShared.Enums.UserRole.SkillProvider)
                await _navigationService.NavigateToAsync("ViewSkillProviderProfileView", new Dictionary<string, object> { { "SkillProviderId", dto.ConcernedId } });
        }

        private async Task LoadAllData()
        {
            if (IsBusy && !ForceReset) return;

            IsBusy = true;
            try
            {
                var userId = _sessionService.GetCurrentUserId();
                var userRole = _sessionService.GetCurrentUserRole();

                var response = await _offerApplicationServiceClient.GetOfferApplicationOfUser(userId, userRole);

                if (response.Success && response.Data != null)
                {
                    _allData = response.Data;

                    // -----------------------------------------------------
                    // 1. Populate Windows Lists (Separate Collections)
                    // -----------------------------------------------------
                    SentOffersPending.Clear();
                    if (_allData.SentPending != null)
                        foreach (var item in _allData.SentPending) SentOffersPending.Add(item);

                    SentOffersHistory.Clear();
                    if (_allData.SentHistory != null)
                        foreach (var item in _allData.SentHistory) SentOffersHistory.Add(item);

                    ReceivedApplicationsPending.Clear();
                    if (_allData.ReceivedPending != null)
                        foreach (var item in _allData.ReceivedPending) ReceivedApplicationsPending.Add(item);

                    ReceivedApplicationsHistory.Clear();
                    if (_allData.ReceivedHistory != null)
                        foreach (var item in _allData.ReceivedHistory) ReceivedApplicationsHistory.Add(item);

                    // -----------------------------------------------------
                    // 2. Populate Android List (Based on Toggle)
                    // -----------------------------------------------------
                    await UpdateDisplayedCards();

                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                await _dialogService.ShowAlertAsync("Error", $"An error occurred while fetching data: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
                ForceReset = false;
            }
        }

        private async Task UpdateDisplayedCards()
        {
            if (_allData == null) return;

            var itemsToShow = await Task.Run(() =>
            {
                if (SelectedTopToggle == "Sent" && SelectedBottomToggle == "Pending")
                    return _allData.SentPending ?? new List<OfferApplicationDisplayDto>();
                else if (SelectedTopToggle == "Sent" && SelectedBottomToggle == "History")
                    return _allData.SentHistory ?? new List<OfferApplicationDisplayDto>();
                else if (SelectedTopToggle == "Received" && SelectedBottomToggle == "Pending")
                    return _allData.ReceivedPending ?? new List<OfferApplicationDisplayDto>();
                else
                    return _allData.ReceivedHistory ?? new List<OfferApplicationDisplayDto>();
            });

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                OfferApplicationCards.Clear();
            });

            const int batchSize = 15;

            await Task.Run(async () =>
            {
                for (int i = 0; i < itemsToShow.Count; i += batchSize)
                {
                    var batch = itemsToShow.Skip(i).Take(batchSize).ToList();

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var item in batch)
                        {
                            OfferApplicationCards.Add(item);
                        }
                    });

                    if (i + batchSize < itemsToShow.Count)
                    {
                        await Task.Delay(5);
                    }
                }
            });
        }
    }
}