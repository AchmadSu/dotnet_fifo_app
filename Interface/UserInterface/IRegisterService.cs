using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.UserDTO;
using FifoApi.Models;

namespace FifoApi.Interface.UserInterface
{
    public interface IRegisterService
    {
        Task<OperationResult<GlobalSuccessResponseDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}