using AutoMapper;
using PLinkageShared.DTOs;
using PLinkageAPI.Models;

namespace PLinkageAPI.Services
{
    // AutoMapper Profile
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Existing Map (SkillProvider -> SkillProviderDto)
            // AutoMapper will use this when you map the top-level object.
            CreateMap<SkillProvider, SkillProviderDto>();

            // 2. 🔑 NEW REQUIRED MAP for the nested 'Education' collection!
            // You must tell AutoMapper how to convert the inner Education model to the inner Education DTO.
            CreateMap<Education, EducationDto>();
            CreateMap<Skill, SkillDto>();

            // Add any other necessary nested maps (e.g., Address, Skill, etc.)
            // CreateMap<PLinkageAPI.Models.Other, PLinkageShared.DTOs.OtherDto>(); 
        }
    }
}
