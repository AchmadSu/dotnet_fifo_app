using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.User;

namespace FifoApi.Interface.User
{
    public interface ILoginService
    {
        Task<OperationResult<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO);
    }
}