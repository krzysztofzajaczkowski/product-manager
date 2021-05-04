using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Domain;

namespace ProductManager.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string email);
        Task AddAsync(User user);
        Task<Role> GetRoleAsync(Guid id);
        Task<Role> GetRoleAsync(string name);
        Task AddAsync(Role role);
    }
}
