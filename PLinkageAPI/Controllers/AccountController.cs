using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageShared.DTOs;
using AutoMapper;
using PLinkageShared.ApiResponse;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var response = await _accountService.AuthenticateUserAsync(loginRequest.UserEmail, loginRequest.UserPassword);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<bool>.Fail("Invalid project data."));

            var response = await _accountService.RegisterUserAsync(registerUserDto);

            if (!response.Success)
                return BadRequest(response);

            var userId = response.Data;
            var userRole = await _accountService.DetermineUserRoleAsync(userId);


            if(userRole == PLinkageShared.Enums.UserRole.SkillProvider)
            {
                var resourceUri = $"/api/SkillProvider/{userId}";
                return Created(resourceUri, response);
            }
            else
            {
                var resourceUri = $"/api/ProjectOwner/{userId}";
                return Created(resourceUri, response);
            }
        }

        [HttpPost("checkemail")]
        public async Task<IActionResult> CheckEmailUniqueness([FromBody] string email)
        {
            var response = await _accountService.CheckEmailUniquenessAsync(email);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        //[HttpPost("migrate-passwords")]
        //public async Task<IActionResult> MigratePasswords()
        //{
        //    var result = await _accountService.HashAllPasswordsAsync();
        //    if (!result.Success)
        //        return BadRequest(result);
        //    return Ok(result);
        //}

    }
}
