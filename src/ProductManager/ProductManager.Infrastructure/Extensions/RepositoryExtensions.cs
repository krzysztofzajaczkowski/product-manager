using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;

namespace ProductManager.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<User> GetOrFailAsync(this IUserRepository repository, Guid id)
        {
            var user = await repository.GetUserAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id: '{id}' does not exist.");
            }

            return user;
        }

        public static async Task<Role> GetRoleOrFailAsync(this IUserRepository repository, Guid id)
        {
            var role = await repository.GetRoleAsync(id);
            if (role == null)
            {
                throw new RoleNotFoundException("Specified role does not exist.");
            }

            return role;
        }

        public static async Task<Role> GetRoleOrFailAsync(this IUserRepository repository, string roleName)
        {
            var role = await repository.GetRoleAsync(roleName);
            if (role == null)
            {
                throw new RoleNotFoundException("Specified role does not exist.");
            }

            return role;
        }
    }
}
