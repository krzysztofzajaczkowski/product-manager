using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class UserServiceTests : TestServerBase
    {
        [Fact]
        public async Task RetrievingAccountByUserId_WhenUserExists_ShouldReturnAccountDto()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Username", "user@email.com", "secret");
            var account = new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Roles = new List<string>
                {
                    "user"
                }
            };
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(user.Id, user.Email, user.Name, user.Password);

            // Act
            var retrievedAccount = await service.GetAccountAsync(user.Id);

            // Assert
            retrievedAccount.Should().BeEquivalentTo(account, opt => opt.ExcludingMissingMembers());
        }



        [Fact]
        public void RetrievingUserById_WhenUserDoesNotExist_ShouldThrowUserNotFoundException()
        {
            // Arrange
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();

            // Act
            Func<Task> action = async () =>
            {
                var account = await service.GetAccountAsync(Guid.NewGuid());
            };

            // Assert
            action.Should().Throw<UserNotFoundException>();
        }

        [Fact]
        public void RegisteringNewUser_WhenRoleDoesNotExists_ShouldThrowRoleNotFoundException()
        {
            // Arrange
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();

            // Act
            Func<Task> action = async () =>
            {
                await service.RegisterAsync(Guid.NewGuid(), "user@email.com", "Username", "secret", "randomRole");
            };

            // Assert
            action.Should().Throw<RoleNotFoundException>();
        }

        [Fact]
        public async Task RegisteringNewUser_WhenUserWithSameEmailExists_ShouldThrowEmailAlreadyUsedException()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string roleName = "user";
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(Guid.NewGuid(), userEmail, "Username", "secret", roleName);

            // Act
            Func<Task> action = async () =>
            {
                await service.RegisterAsync(Guid.NewGuid(), userEmail, "Username", "secret", roleName);
            };

            // Assert
            action.Should().Throw<EmailAlreadyUsedException>();
        }

        [Fact]
        public void LoginUser_WhenUserDoesNotExist_ShouldThrowInvalidCredentialsException()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();

            // Act
            Func<Task> action = async () =>
            {
                await service.LoginAsync(userEmail, userPassword, roleName);
            };

            // Assert
            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public async Task LoginUser_WhenUserExistsAndCredentialsAreInvalid_ShouldThrowInvalidCredentialsException()
        {
            // Arrange
            const string userName = "Username";
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            var role = new Role(roleName);
            var user = new User(Guid.NewGuid(), userName, userEmail, userPassword, role);
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(user.Id, user.Email, user.Name, user.Password, roleName);

            // Act
            Func<Task> action = async () =>
            {
                await service.LoginAsync(userEmail, "", roleName);
            };

            // Assert
            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public async Task LoginUser_WhenUserExistsAndCredentialsAreValidAndUserDoesNotHaveSpecifiedRole_ShouldThrowInvalidCredentialsException()
        {
            // Arrange
            const string userName = "Username";
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            var role = new Role(roleName);
            var user = new User(Guid.NewGuid(), userName, userEmail, userPassword, role);
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(user.Id, user.Email, user.Name, user.Password, roleName);

            // Act
            Func<Task> action = async () =>
            {
                await service.LoginAsync(userEmail, userPassword, "");
            };

            // Assert
            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public async Task LoginUser_WhenUserExistsAndCredentialsAreValid_ShouldReturnJwtDto()
        {
            // Arrange
            const string userName = "Username";
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            var role = new Role(roleName);
            var user = new User(Guid.NewGuid(), userName, userEmail, userPassword, role);
            using var server = BuildTestServer();
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(user.Id, user.Email, user.Name, user.Password, roleName);

            // Act
            var account = await service.LoginAsync(userEmail, userPassword, roleName);

            // Assert
            account.Should().NotBeNull();
        }


    }
}
