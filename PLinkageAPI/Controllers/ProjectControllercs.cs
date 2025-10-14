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

            var projectDto = _mapper.Map<ProjectDto>(response.Data);
            return Ok(ApiResponse<ProjectDto>.Ok(projectDto, response.Message));
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectDto projectCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<bool>.Fail("Invalid project data."));

            var response = await _projectService.AddProjectAsync(projectCreationDto);
            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetSpecific),
                new { projectId = projectCreationDto.ProjectId },
                response);
        }

        [HttpPut("{projectId}")]
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

            var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(response.Data);
            return Ok(ApiResponse<IEnumerable<ProjectDto>>.Ok(projectDtos, response.Message));
        }
    }
}
