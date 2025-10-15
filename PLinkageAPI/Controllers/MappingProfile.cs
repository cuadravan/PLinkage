using AutoMapper;
using PLinkageAPI.Entities;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Controllers
{
    // AutoMapper Profile
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SkillProvider, SkillProviderDto>();
            CreateMap<SkillProviderDto, SkillProvider>();

            CreateMap<Education, EducationDto>();
            CreateMap<EducationDto, Education>();
            CreateMap<Skill, SkillDto>();
            CreateMap<SkillDto, Skill>();

            CreateMap<ProjectOwner, ProjectOwnerDto>();
            CreateMap<ProjectOwnerDto, ProjectOwner>();

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();

        }
    }
}
