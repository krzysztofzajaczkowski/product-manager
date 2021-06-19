using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Specialized;
using Moq;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Helper;
using ProductManager.Infrastructure.Repositories;
using ProductManager.Infrastructure.Services;
using Xunit;

namespace ProductManager.UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task RetrievingAccountByUserId_WhenUserExists_ShouldReturnAccountDto()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Username", "user@email.com", "secret");
            var account = new AccountDto
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            var retrievedAccount = await service.GetAccountAsync(user.Id);

            // Assert
            retrievedAccount.Should().BeEquivalentTo(account);
        }

        [Fact]
        public async Task RetrievingAccountByUserId_WhenUserExists_ShouldCallRepositoryGetUserAsyncOnce()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Username", "user@email.com", "secret");
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            await service.GetAccountAsync(user.Id);

            // Assert
            repository.Verify(x => x.GetUserAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void RetrievingUserById_WhenUserDoesNotExist_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            Func<Task> action = async () =>
            {
                var account = await service.GetAccountAsync(Guid.NewGuid());
            };

            // Assert
            action.Should().Throw<UserNotFoundException>();
            repository.Verify(x => x.GetUserAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void RegisteringNewUser_WhenRoleDoesNotExists_ShouldThrowRoleNotFoundException()
        {
            // Arrange
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync((Role)null);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            Func<Task> action = async () =>
            {
                await service.RegisterAsync(Guid.NewGuid(), "user@email.com", "Username", "secret", "randomRole");
            };

            // Assert
            action.Should().Throw<RoleNotFoundException>();
            repository.Verify(x => x.GetRoleAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void RegisteringNewUser_WhenUserWithSameEmailExists_ShouldThrowEmailAlreadyUsedException()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string roleName = "user";
            var user = new User(Guid.NewGuid(), "Username", userEmail, "secret");
            var role = new Role(roleName);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetRoleAsync(roleName)).ReturnsAsync(role);
            repository.Setup(x => x.GetUserAsync(userEmail)).ReturnsAsync(user);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            Func<Task> action = async () =>
            {
                await service.RegisterAsync(Guid.NewGuid(), userEmail, "Username", "secret", roleName);
            };

            // Assert
            action.Should().Throw<EmailAlreadyUsedException>();
            repository.Verify(x => x.GetRoleAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RegisteringNewUser_WhenEmailIsNotAlreadyUsed_ShouldCallRepositoryAddAsyncOnce()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string roleName = "user";
            var role = new Role(roleName);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetRoleAsync(roleName)).ReturnsAsync(role);
            repository.Setup(x => x.GetUserAsync(userEmail)).ReturnsAsync((User)null);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            await service.RegisterAsync(Guid.NewGuid(), userEmail, "Username", "secret", roleName);

            // Assert
            repository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisteringNewUser_WhenEmailIsNotAlreadyUsed_ShouldCallRepositoryGetRoleAsyncOnce()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string roleName = "user";
            var role = new Role(roleName);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetRoleAsync(roleName)).ReturnsAsync(role);
            repository.Setup(x => x.GetUserAsync(userEmail)).ReturnsAsync((User)null);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            await service.RegisterAsync(Guid.NewGuid(), userEmail, "Username", "secret", roleName);

            // Assert
            repository.Verify(x => x.GetRoleAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RegisteringNewUser_WhenEmailIsNotAlreadyUsed_ShouldCallRepositoryGetUserAsyncOnce()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string roleName = "user";
            var role = new Role(roleName);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            repository.Setup(x => x.GetRoleAsync(roleName)).ReturnsAsync(role);
            repository.Setup(x => x.GetUserAsync(userEmail)).ReturnsAsync((User)null);
            var service = new UserService(repository.Object, jwtHandler.Object);

            // Act
            await service.RegisterAsync(Guid.NewGuid(), userEmail, "Username", "secret", roleName);

            // Assert
            repository.Verify(x => x.GetUserAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void LoginUser_WhenUserDoesNotExist_ShouldThrowInvalidCredentialsException()
        {
            // Arrange
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            var service = new UserService(repository.Object, jwtHandler.Object);
            repository.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            Func<Task> action = async () =>
            {
                await service.LoginAsync(userEmail, userPassword, roleName);
            };

            // Assert
            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void LoginUser_WhenUserExistsAndCredentialsAreInvalid_ShouldThrowInvalidCredentialsException()
        {
            // Arrange
            const string userName = "Username";
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            var role = new Role(roleName);
            var user = new User(Guid.NewGuid(), userName, userEmail, userPassword, role);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            var service = new UserService(repository.Object, jwtHandler.Object);
            repository.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            Func<Task> action = async () =>
            {
                await service.LoginAsync(userEmail, "", roleName);
            };

            // Assert
            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void LoginUser_WhenUserExistsAndCredentialsAreValidAndUserDoesNotHaveSpecifiedRole_ShouldThrowInvalidCredentialsException()
        {
            // Arrange
            const string userName = "Username";
            const string userEmail = "user@email.com";
            const string userPassword = "secret";
            const string roleName = "user";
            var role = new Role(roleName);
            var user = new User(Guid.NewGuid(), userName, userEmail, userPassword, role);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            var service = new UserService(repository.Object, jwtHandler.Object);
            repository.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

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
            var jwtDto = new JwtDto
            {
                Expires = 111,
                Token = "token",
                Role = roleName
            };
            var role = new Role(roleName);
            var user = new User(Guid.NewGuid(), userName, userEmail, PasswordHelper.CalculateHash(userPassword), role);
            var repository = new Mock<IUserRepository>();
            var jwtHandler = new Mock<IJwtHandler>();
            var service = new UserService(repository.Object, jwtHandler.Object);
            repository.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            jwtHandler.Setup(x => x.CreateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(jwtDto);

            // Act
            var account = await service.LoginAsync(userEmail, userPassword, roleName);

            // Assert
            account.Should().NotBeNull().And.BeEquivalentTo(jwtDto);
        }
    }
}
