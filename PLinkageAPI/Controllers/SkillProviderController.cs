using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PLinkageShared.ApiResponse;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillProviderController : ControllerBase
    {
        private readonly ISkillProviderService _skillProviderService;
        private readonly IMapper _mapper;

        public SkillProviderController(ISkillProviderService skillProviderService, IMapper mapper)
        {
            _skillProviderService = skillProviderService;
            _mapper = mapper;
        }

        [HttpGet("{skillProviderId}")]
        public async Task<IActionResult> GetSpecific(Guid skillProviderId)
        {
            var response = await _skillProviderService.GetSpecificSkillProviderAsync(skillProviderId);

            if (!response.Success)
                return NotFound(response);

            var dto = _mapper.Map<SkillProviderDto>(response.Data);
            return Ok(ApiResponse<SkillProviderDto>.Ok(dto, response.Message));
        }

        [HttpPut("{skillProviderId}")]
        public async Task<IActionResult> UpdateSkillProvider(Guid skillProviderId, [FromBody] UserProfileUpdateDto updateDto)
        {
            var response = await _skillProviderService.UpdateSkillProviderAsync(skillProviderId, updateDto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string proximity = "All",
            [FromQuery] CebuLocation? location = null,
            [FromQuery] string status = "All")
        {
            var response = await _skillProviderService.GetFilteredSkillProvidersAsync(proximity, location, status);
            if (!response.Success)
                return NotFound(response);
            return Ok(ApiResponse<IEnumerable<SkillProviderCardDto>>.Ok(response.Data, response.Message));
        }

        // ----------------- EDUCATIONS -----------------

        [HttpPost("{skillProviderId}/educations")]
        public async Task<IActionResult> AddEducation(Guid skillProviderId, [FromBody] EducationDto educationDto)
        {
            var education = _mapper.Map<Education>(educationDto);
            var response = await _skillProviderService.AddEducationAsync(skillProviderId, education);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPut("{skillProviderId}/educations/{index}")]
        public async Task<IActionResult> UpdateEducation(Guid skillProviderId, int index, [FromBody] EducationDto dto)
        {
            var education = _mapper.Map<Education>(dto);
            var response = await _skillProviderService.UpdateEducationAsync(skillProviderId, index, education);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{skillProviderId}/educations/{index}")]
        public async Task<IActionResult> DeleteEducation(Guid skillProviderId, int index)
        {
            var response = await _skillProviderService.DeleteEducationAsync(skillProviderId, index);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // ----------------- SKILLS -----------------

        [HttpPost("{skillProviderId}/skills")]
        public async Task<IActionResult> AddSkill(Guid skillProviderId, [FromBody] SkillDto dto)
        {
            var skill = _mapper.Map<Skill>(dto);
            var response = await _skillProviderService.AddSkillAsync(skillProviderId, skill);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPut("{skillProviderId}/skills/{index}")]
        public async Task<IActionResult> UpdateSkill(Guid skillProviderId, int index, [FromBody] SkillDto dto)
        {
            var skill = _mapper.Map<Skill>(dto);
            var response = await _skillProviderService.UpdateSkillAsync(skillProviderId, index, skill);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{skillProviderId}/skills/{index}")]
        public async Task<IActionResult> DeleteSkill(Guid skillProviderId, int index)
        {
            var response = await _skillProviderService.DeleteSkillAsync(skillProviderId, index);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
