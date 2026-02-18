using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.Interface.User;
using FifoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());
        }

        public async Task<AppUser?> GetByIdAsync(string id)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IsExistEmailAsync(string email)
        {
            return await _context.AppUsers.AnyAsync(u => u.NormalizedEmail == email.ToUpper());

        }
    }
}