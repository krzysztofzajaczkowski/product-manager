using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProductManager.Core.Domain;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Database;

namespace ProductManager.Infrastructure.Repositories
{
    public class SQLiteUserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public SQLiteUserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task<User> GetUserAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            var user = (await connection.QueryAsync<User, Role, User>(
                    @"SELECT * FROM Users users
                        INNER JOIN UserRoles userRoles ON users.Id = userRoles.UserId
                        INNER JOIN Roles roles ON userRoles.RoleId = roles.Id
                        WHERE users.Id = @Id", (user, role) =>
                    {
                        user.AddRole(role);
                        return user;
                    },
                    new
                    {
                        Id = id
                    })).GroupBy(u => u.Id)
                .Select(g =>
                {
                    var combinedUser = g.First();
                    var roles = g.Skip(1).Select(u => u.Roles.Single());
                    foreach (var role in roles)
                    {
                        combinedUser.AddRole(role);
                    }

                    return combinedUser;
                }).First();

            return user;
        }

        public async Task<User> GetUserAsync(string email)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            var user = (await connection.QueryAsync<User, Role, User>(
                    @"SELECT * FROM Users users
                        INNER JOIN UserRoles userRoles ON users.Id = userRoles.UserId
                        INNER JOIN Roles roles ON userRoles.RoleId = roles.Id
                        WHERE Email = @Email", (user, role) =>
                    {
                        user.AddRole(role);
                        return user;
                    },
                    new
                    {
                        Email = email
                    })).GroupBy(u => u.Id)
                .Select(g =>
                {
                    var combinedUser = g.First();
                    var roles = g.Skip(1).Select(u => u.Roles.Single());
                    foreach (var role in roles)
                    {
                        combinedUser.AddRole(role);
                    }

                    return combinedUser;
                }).First();


            return user;
        }

        public async Task AddAsync(User user)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            await connection.ExecuteAsync("INSERT INTO Users(Id, Email, Name, Password) VALUES (@Id, @Email, @Name, @Password);",
                user);

            foreach (var userRole in user.Roles)
            {
                var role = await GetRoleAsync(userRole.Name);

                if (role == null)
                {
                    //await connection.ExecuteAsync("INSERT INTO Roles(Id, Name) VALUES (@id, @name);", userRole);
                    await AddAsync(userRole);
                    role = userRole;
                }

                await connection.ExecuteAsync("INSERT INTO UserRoles(UserId, RoleId) VALUES(@UserId, @RoleId);", new
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }
        }

        public async Task<Role> GetRoleAsync(Guid id)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            var role = await connection.QuerySingleOrDefaultAsync<Role>("SELECT * FROM Roles WHERE Id = @Id;",
                new { Id = id });
            return role;
        }

        public async Task<Role> GetRoleAsync(string name)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();

            var role = await connection.QuerySingleOrDefaultAsync<Role>("SELECT * FROM Roles WHERE Name = @Name;",
                new { Name = name });
            return role;
        }

        public async Task AddAsync(Role role)
        {
            using var connection = await _dbConnectionFactory.CreateAsync();
            await connection.ExecuteAsync("INSERT INTO Roles(Id, Name) VALUES (@Id, @Name);", role);
        }
    }
}
