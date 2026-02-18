using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Models;

namespace FifoApi.Interface.User
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}