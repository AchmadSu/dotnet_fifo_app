using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.UserDTO;
using FifoApi.Models;

namespace FifoApi.Mappers.UserMapper
{
    public static class AccountMapper
    {
        public static LoginResponseDTO ToLoginResponse(this AppUser appUser, string token)
        {
            return new LoginResponseDTO
            {
                Username = appUser.UserName,
                Email = appUser.Email,
                Token = token
            };
        }

        public static AppUser FromRegisterDtoToAppUser(this RegisterDTO registerDTO)
        {
            return new AppUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };
        }
    }
}