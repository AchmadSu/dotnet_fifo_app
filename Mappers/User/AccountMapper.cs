using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.User;
using FifoApi.Models;

namespace FifoApi.Mappers.User
{
    public static class AccountMapper
    {
        public static LoginResponseDTO toLoginResponse(this AppUser appUser, string token)
        {
            return new LoginResponseDTO
            {
                Username = appUser.UserName,
                Email = appUser.Email,
                Token = token
            };
        }

        public static AppUser fromRegisterDtoToAppUser(this RegisterDTO registerDTO)
        {
            return new AppUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };
        }
    }
}