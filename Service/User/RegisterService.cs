using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.User;
using FifoApi.Interface.User;
using FifoApi.Mappers.User;
using FifoApi.Models;
using Microsoft.AspNetCore.Identity;

namespace FifoApi.Service.User
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterService> _logger;
        public RegisterService(
            IUserRepository userRepo,
            UserManager<AppUser> userManager,
            ILogger<RegisterService> logger
        )
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<OperationResult<GlobalSuccessResponseDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var existEmail = await _userRepo.IsExistEmailAsync(registerDTO.Email);

                if (existEmail)
                    return OperationResult<GlobalSuccessResponseDTO>.BadRequest("Create user failed", new string[] { "Email has been taken!" });

                var appUser = registerDTO.fromRegisterDtoToAppUser();

                var createdUser = await _userManager.CreateAsync(appUser, registerDTO.Password);

                if (!createdUser.Succeeded)
                {
                    var errors = createdUser.Errors.Select(e => e.Description).ToArray();
                    return OperationResult<GlobalSuccessResponseDTO>.BadRequest("Create user failed", errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors.Select(e => e.Description).ToArray();
                    return OperationResult<GlobalSuccessResponseDTO>.BadRequest("Create user failed", errors);
                }

                return OperationResult<GlobalSuccessResponseDTO>.Ok(
                    new GlobalSuccessResponseDTO
                    {
                        Message = "User registered successfully!"
                    }
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while registering user");
                return OperationResult<GlobalSuccessResponseDTO>.InternalServerError();
            }
        }

    }
}