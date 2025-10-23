using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using AutoMapper;
using PLinkageShared.ApiResponse;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetSpecific(Guid projectId)
        {
            var response = await _projectService.GetSpecificProjectAsync(projectId);
            if (!response.Success)
                return NotFound(response);

            return Ok(ApiResponse<ProjectDto>.Ok(response.Data, response.Message));
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectCreationDto projectCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<bool>.Fail("Invalid project data."));

            var response = await _projectService.AddProjectAsync(projectCreationDto);
            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetSpecific),
                new { projectId = response.Data },
                response);
        }

        [HttpPatch("{projectId}")]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectUpdateDto projectUpdateDto)
        {
            var response = await _projectService.UpdateProjectAsync(projectUpdateDto);
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
            var response = await _projectService.GetFilteredProjectsAsync(proximity, location, status);
            if (!response.Success)
                return NotFound(response);

            return Ok(ApiResponse<IEnumerable<ProjectCardDto>>.Ok(response.Data, response.Message));
        }

        [HttpPost("requestresignation")]
        public async Task<IActionResult> RequestResignationAsync([FromBody] RequestResignationDto requestResignationDto)
        {
            var response = await _projectService.RequestResignation(requestResignationDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("processresignation")]
        public async Task<IActionResult> ProcessResignationAsync([FromBody] ProcessResignationDto processResignationDto)
        {
            var response = await _projectService.ProcessResignation(processResignationDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("ratings")]
        public async Task<IActionResult> RateSkillProvidersAsync([FromBody] RateSkillProviderDto rateSkillProviderDto)
        {
            var response = await _projectService.RateSkillProviders(rateSkillProviderDto);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
