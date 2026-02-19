using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.User;
using FifoApi.Models;

namespace FifoApi.Interface.User
{
    public interface IRegisterService
    {
        Task<OperationResult<GlobalSuccessResponseDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}