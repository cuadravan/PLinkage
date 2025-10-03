using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Models; // Assuming CebuLocation is here
using PLinkageShared.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using PLinkageShared.Enums;
using AutoMapper;

namespace PLinkageAPI.Controllers
{
    // Make sure your controller uses the correct route attributes
    [Route("api/[controller]")]
    [ApiController]
    public class SkillProvidersController : ControllerBase
    {
        // 1. Inject the Service Interface
        private readonly ISkillProviderService _skillProviderService;
        private readonly IMapper _mapper;

        public SkillProvidersController(ISkillProviderService skillProviderService, IMapper mapper)
        {
            _skillProviderService = skillProviderService;
            _mapper = mapper;
        }

        // The endpoint is cleaner and only deals with request parameters and responses.
        // All filtering logic is abstracted away in the service/specification layer.
        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string proximity = "All", // Defaults to "All"
            [FromQuery] CebuLocation? location = null,
            [FromQuery] string status = "All") // Assuming a default for status
        {
            //try
            //{
                // 2. Call the service method, passing the raw request parameters.
                // The service handles the location calculation, specification creation, and repository call.
                IEnumerable<SkillProvider> filteredSkillProviders =
                    await _skillProviderService.GetFilteredProvidersAsync(
                        proximity,
                        location,
                        status
                    );

                // Assuming DTO mapping or result shaping happens here or in the service,
                // but for this example, we return the entities directly.
                if (filteredSkillProviders == null || !filteredSkillProviders.Any())
                {
                    return NotFound("No skill providers found matching the criteria.");
                }

                var skillproviderDtos = _mapper.Map <IEnumerable<SkillProviderDto>>(filteredSkillProviders);

                return Ok(filteredSkillProviders);
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception (recommended)
            //    return StatusCode(500, "An error occurred while fetching filtered providers.");
            //}
        }
    }
}