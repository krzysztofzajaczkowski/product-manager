using System;
using System.Collections.Generic;
using System.Text;
using ProductManager.Infrastructure.DTO;

namespace ProductManager.Infrastructure.Services
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(Guid userId, string role);
    }
}
