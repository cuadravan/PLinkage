using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Entities;
using PLinkageShared.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using PLinkageShared.Enums;
using AutoMapper;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillProvidersController : ControllerBase
    {
        private readonly ISkillProviderService _skillProviderService;
        private readonly IMapper _mapper;

        public SkillProvidersController(ISkillProviderService skillProviderService, IMapper mapper)
        {
            _skillProviderService = skillProviderService;
            _mapper = mapper;
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string proximity = "All",
            [FromQuery] CebuLocation? location = null,
            [FromQuery] string status = "All")
        {
            try
            {
                IEnumerable<SkillProvider> filteredSkillProviders =
                    await _skillProviderService.GetFilteredProvidersAsync(
                        proximity,
                        location,
                        status
                    );

                if (filteredSkillProviders == null || !filteredSkillProviders.Any())
                {
                    return NotFound("No skill providers found matching the criteria.");
                }

                var skillproviderDtos = _mapper.Map<IEnumerable<SkillProviderDto>>(filteredSkillProviders);

                return Ok(filteredSkillProviders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching filtered providers. Error: {ex}");
            }
        }
    }
}