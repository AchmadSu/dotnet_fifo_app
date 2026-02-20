using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.UserDTO;

namespace FifoApi.Interface.UserInterface
{
    public interface ILoginService
    {
        Task<OperationResult<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO);
    }
}