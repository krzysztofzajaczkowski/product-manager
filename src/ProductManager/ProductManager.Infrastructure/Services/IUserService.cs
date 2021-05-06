using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Infrastructure.DTO;

namespace ProductManager.Infrastructure.Services
{
    public interface IUserService
    {
        Task<AccountDto> GetAccountAsync(Guid userId);
        Task RegisterAsync(Guid userId, string email,
            string name, string password, string roleName = "user");
        Task<JwtDto> LoginAsync(string email, string password, string role);
    }
}
