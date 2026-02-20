using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Models;

namespace FifoApi.Interface.UserInterface
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByIdAsync(string id);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<bool> IsExistEmailAsync(string email);
    }
}