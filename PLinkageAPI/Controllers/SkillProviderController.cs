using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageShared.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using PLinkageShared.Enums;
using AutoMapper;
using System.Linq;
using System;

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
            try
            {
                var skillProvider = await _skillProviderService.GetSpecificSkillProviderAsync(skillProviderId);
                if (skillProvider == null)
                    return NotFound("Requested skill provider with ID not found.");

                var skillProviderDto = _mapper.Map<SkillProviderDto>(skillProvider);
                return Ok(skillProviderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the skill provider. Error: {ex.Message}");
            }
        }

        [HttpPut("{skillProviderId}")]
        public async Task<IActionResult> UpdateSkillProvider(Guid skillProviderId, [FromBody] SkillProviderUpdateDto skillProviderUpdateDto)
        {
            try
            {
                bool isSuccess = await _skillProviderService.UpdateSkillProviderAsync(skillProviderId, skillProviderUpdateDto);        

                if (isSuccess)
                    return NoContent();

                return NotFound($"Skill provider with ID no. {skillProviderId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating education. Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string proximity = "All",
            [FromQuery] CebuLocation? location = null,
            [FromQuery] string status = "All")
        {
            try
            {
                var filteredSkillProviders = await _skillProviderService.GetFilteredSkillProvidersAsync(
                    proximity, location, status);

                if (filteredSkillProviders == null || !filteredSkillProviders.Any())
                    return NotFound("No skill providers found matching the criteria.");

                var skillProviderDtos = _mapper.Map<IEnumerable<SkillProviderDto>>(filteredSkillProviders);
                return Ok(skillProviderDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching filtered providers. Error: {ex.Message}");
            }
        }

        // ----------------- EDUCATIONS -----------------

        // POST: Add education
        [HttpPost("{skillProviderId}/educations")]
        public async Task<IActionResult> AddEducation(Guid skillProviderId, [FromBody] EducationDto educationToAdd)
        {
            try
            {
                var educationEntity = _mapper.Map<Education>(educationToAdd);
                bool isSuccess = await _skillProviderService.AddEducationAsync(skillProviderId, educationEntity);

                if (isSuccess)
                    return CreatedAtAction(nameof(GetSpecific), new { skillProviderId }, null);

                return StatusCode(500, "An error occurred while adding education.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding education. Error: {ex.Message}");
            }
        }

        // PUT: Update education by index
        [HttpPut("{skillProviderId}/educations/{index}")]
        public async Task<IActionResult> UpdateEducation(Guid skillProviderId, int index, [FromBody] EducationDto updatedEducation)
        {
            try
            {
                var educationEntity = _mapper.Map<Education>(updatedEducation);
                bool isSuccess = await _skillProviderService.UpdateEducationAsync(skillProviderId, index, educationEntity);

                if (isSuccess)
                    return NoContent();

                return NotFound($"Education at index {index} not found for SkillProvider {skillProviderId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating education. Error: {ex.Message}");
            }
        }

        // DELETE: Delete education by index
        [HttpDelete("{skillProviderId}/educations/{index}")]
        public async Task<IActionResult> DeleteEducation(Guid skillProviderId, int index)
        {
            try
            {
                bool isSuccess = await _skillProviderService.DeleteEducationAsync(skillProviderId, index);

                if (isSuccess)
                    return NoContent();

                return NotFound($"Education at index {index} not found for SkillProvider {skillProviderId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting education. Error: {ex.Message}");
            }
        }

        // ----------------- Skills -----------------

        // POST: Add skill
        [HttpPost("{skillProviderId}/skills")]
        public async Task<IActionResult> AddSkill(Guid skillProviderId, [FromBody] SkillDto skillToAdd)
        {
            try
            {
                var skillEntity = _mapper.Map<Skill>(skillToAdd);
                bool isSuccess = await _skillProviderService.AddSkillAsync(skillProviderId, skillEntity);

                if (isSuccess)
                    return CreatedAtAction(nameof(GetSpecific), new { skillProviderId }, null);

                return StatusCode(500, "An error occurred while adding skill.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding skill. Error: {ex.Message}");
            }
        }

        // PUT: Update skill by index
        [HttpPut("{skillProviderId}/skills/{index}")]
        public async Task<IActionResult> UpdateSkill(Guid skillProviderId, int index, [FromBody] SkillDto updatedSkill)
        {
            try
            {
                var skillEntity = _mapper.Map<Skill>(updatedSkill);
                bool isSuccess = await _skillProviderService.UpdateSkillAsync(skillProviderId, index, skillEntity);

                if (isSuccess)
                    return NoContent();

                return NotFound($"Skill at index {index} not found for SkillProvider {skillProviderId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating education. Error: {ex.Message}");
            }
        }

        // DELETE: Delete skill by index
        [HttpDelete("{skillProviderId}/skills/{index}")]
        public async Task<IActionResult> DeleteSkill(Guid skillProviderId, int index)
        {
            try
            {
                bool isSuccess = await _skillProviderService.DeleteSkillAsync(skillProviderId, index);

                if (isSuccess)
                    return NoContent();

                return NotFound($"Skill at index {index} not found for SkillProvider {skillProviderId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting education. Error: {ex.Message}");
            }
        }
    }
}
