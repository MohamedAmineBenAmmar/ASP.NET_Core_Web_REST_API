using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}