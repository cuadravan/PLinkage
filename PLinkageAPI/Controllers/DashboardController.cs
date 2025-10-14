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
using PLinkageAPI.Services;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public DashboardController(IDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        [HttpGet("projectowner/{projectOwnerId}")]
        public async Task<IActionResult> GetProjectOwnerDashboard(Guid projectOwnerId)
        {
            var response = await _dashboardService.GetProjectOwnerDashboardAsync(projectOwnerId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("skillprovider/{skillProviderId}")]
        public async Task<IActionResult> GetSkillProviderDashboard(Guid skillProviderId)
        {
            var response = await _dashboardService.GetSkillProviderDashboardAsync(skillProviderId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("admin/{adminId}")]
        public async Task<IActionResult> GetAdminDashboard(Guid adminId)
        {
            var response = await _dashboardService.GetAdminDashboardAsync(adminId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

    }
}
