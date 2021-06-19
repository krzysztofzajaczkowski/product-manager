using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Domain;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Helper;

namespace ProductManager.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ISet<Role> _roles = new HashSet<Role>();

        private readonly ISet<User> _users = new HashSet<User>();

        public InMemoryUserRepository()
        {
            _roles.Add(new Role(Guid.NewGuid(), "user"));
            _roles.Add(new Role(Guid.NewGuid(), "admin"));
            _roles.Add(new Role(Guid.NewGuid(), "CatalogManager"));
            _roles.Add(new Role(Guid.NewGuid(), "SalesManager"));
            _roles.Add(new Role(Guid.NewGuid(), "WarehouseManager"));

            _users.Add(new User(Guid.NewGuid(), "User", "user@user.com", PasswordHelper.CalculateHash("secret"),
                _roles.SingleOrDefault(r => r.Name == "user")));
            _users.Add(new User(Guid.NewGuid(), "Admin", "admin@admin.com", PasswordHelper.CalculateHash("secret"), _roles.ToArray()));

        }

        public async Task<Role> GetRoleAsync(Guid id)
            => await Task.FromResult(_roles.SingleOrDefault(x => x.Id == id));

        public async Task<Role> GetRoleAsync(string name)
            => await Task.FromResult(_roles.SingleOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)));

        public Task AddAsync(Role role)
        {
            _roles.Add(role);
            return Task.CompletedTask;
        }

        public async Task<User> GetUserAsync(Guid id)
            => await Task.FromResult(_users.SingleOrDefault(x => x.Id == id));

        public async Task<User> GetUserAsync(string email)
            => await Task.FromResult(_users.SingleOrDefault(x =>
                string.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase)));

        public async Task AddAsync(User user)
        {
            var hashedPasswordUser = new User(user.Id, user.Name, user.Email, PasswordHelper.CalculateHash(user.Password), user.Roles.ToArray());
            _users.Add(hashedPasswordUser);
            await Task.CompletedTask;
        }
    }
}
