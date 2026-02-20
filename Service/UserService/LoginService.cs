using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.UserDTO;
using FifoApi.Interface.UserInterface;
using FifoApi.Mappers.UserMapper;
using FifoApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Service.UserService
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginService(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            SignInManager<AppUser> signInManager
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        public async Task<OperationResult<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x =>
                x.UserName.ToLower() == loginDTO.Username.ToLower()
            );

            if (user == null) return OperationResult<LoginResponseDTO>.Unauthorized("Invalid username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return OperationResult<LoginResponseDTO>.Unauthorized("Username not found and/or password incorrect");

            var token = _tokenService.CreateToken(user);

            return OperationResult<LoginResponseDTO>.Ok(
                user.ToLoginResponse(token)
            );
        }
    }
}