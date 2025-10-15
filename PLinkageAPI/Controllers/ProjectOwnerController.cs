using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
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
    public class ProjectOwnerController : ControllerBase
    {
        private readonly IProjectOwnerService _projectOwnerService;
        private readonly IMapper _mapper;

        public ProjectOwnerController(IProjectOwnerService projectOwnerService, IMapper mapper)
        {
            _projectOwnerService = projectOwnerService;
            _mapper = mapper;
        }

        [HttpGet("{projectOwnerId}")]
        public async Task<IActionResult> GetSpecific(Guid projectOwnerId)
        {
            var response = await _projectOwnerService.GetSpecificProjectOwnerAsync(projectOwnerId);

            if (!response.Success)
                return NotFound(response);

            var mappedData = _mapper.Map<ProjectOwnerDto>(response.Data);
            return Ok(ApiResponse<ProjectOwnerDto>.Ok(mappedData, response.Message));
        }

        [HttpPut("{projectOwnerId}")]
        public async Task<IActionResult> UpdateProjectOwner(Guid projectOwnerId, [FromBody] UserProfileUpdateDto projectOwnerUpdateDto)
        {
            var response = await _projectOwnerService.UpdateProjectOwnerAsync(projectOwnerId, projectOwnerUpdateDto);

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
            var response = await _projectOwnerService.GetFilteredProjectOwnerAsync(proximity, location, status);

            if (!response.Success)
                return NotFound(response);
            return Ok(ApiResponse<IEnumerable<ProjectOwnerCardDto>>.Ok(response.Data, response.Message));
        }
    }
}
