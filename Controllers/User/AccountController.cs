using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.User;
using FifoApi.Extensions.Controllers;
using FifoApi.Interface.User;
using FifoApi.Mappers.User;
using FifoApi.Models;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Controllers.User
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ILoginService loginService,
            IRegisterService registerService,
            ILogger<AccountController> logger
        )
        {
            _loginService = loginService;
            _registerService = registerService;
            _logger = logger;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var result = await _loginService.LoginAsync(loginDTO);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while login {Username}", loginDTO.Username);
                return this.ToErrorActionResult();
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                var result = await _registerService.RegisterAsync(registerDTO);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while registering {Email}", registerDTO.Email);
                return this.ToErrorActionResult();
            }
        }
    }
}