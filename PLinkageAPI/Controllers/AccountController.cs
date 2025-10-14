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
            var response = await _accountService.RegisterUserAsync(registerUserDto);

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
