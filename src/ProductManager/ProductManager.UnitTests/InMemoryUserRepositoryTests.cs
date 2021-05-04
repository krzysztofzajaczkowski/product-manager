using System;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Repositories;
using Xunit;

namespace ProductManager.UnitTests
{
    public class InMemoryUserRepositoryTests
    {
        [Fact]
        public async Task RetrievingUserById_WhenUserDoesNotExists_ShouldReturnNull()
        {
            // Arrange
            var repository = new InMemoryUserRepository();

            // Act
            var user = await repository.GetUserAsync(Guid.NewGuid());

            // Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task RetrievingUserByEmail_WhenUserDoesNotExists_ShouldReturnNull()
        {
            // Arrange
            var repository = new InMemoryUserRepository();

            // Act
            var user = await repository.GetUserAsync($"Test{Guid.NewGuid()}");

            // Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task AddingUser_WhenRetrievedById_RepositoryShouldContainUser()
        {
            // Arrange
            var repository = new InMemoryUserRepository();
            var createdUser = new User(Guid.NewGuid(), "Username", "user@email.com", "secret");
            await repository.AddAsync(createdUser);

            // Act
            var retrievedUser = await repository.GetUserAsync(createdUser.Id);

            // Assert
            retrievedUser.Should().Be(createdUser);
        }

        [Fact]
        public async Task AddingUser_WhenRetrievedByEmail_RepositoryShouldContainUser()
        {
            // Arrange
            var repository = new InMemoryUserRepository();
            var createdUser = new User(Guid.NewGuid(), "Username", "user@email.com", "secret");
            await repository.AddAsync(createdUser);

            // Act
            var retrievedUser = await repository.GetUserAsync(createdUser.Email);

            // Assert
            retrievedUser.Should().Be(createdUser);
        }

        [Fact]
        public async Task RetrievingRoleById_WhenRoleDoesNotExists_ShouldReturnNull()
        {
            // Arrange
            var repository = new InMemoryUserRepository();

            // Act
            var role = await repository.GetRoleAsync(Guid.NewGuid());

            // Assert
            role.Should().BeNull();
        }

        [Fact]
        public async Task RetrievingRoleByName_WhenRoleDoesNotExists_ShouldReturnNull()
        {
            // Arrange
            var repository = new InMemoryUserRepository();

            // Act
            var role = await repository.GetRoleAsync($"Test{Guid.NewGuid()}");

            // Assert
            role.Should().BeNull();
        }

        [Fact]
        public async Task AddingRole_WhenRetrievedById_RepositoryShouldContainRole()
        {
            // Arrange
            var repository = new InMemoryUserRepository();
            var createdRole = new Role(Guid.NewGuid(), "Rolename");
            await repository.AddAsync(createdRole);

            // Act
            var retrievedRole = await repository.GetRoleAsync(createdRole.Id);

            // Assert
            retrievedRole.Should().Be(createdRole);
        }

        [Fact]
        public async Task AddingRole_WhenRetrievedByName_RepositoryShouldContainRole()
        {
            // Arrange
            var repository = new InMemoryUserRepository();
            var createdRole = new Role(Guid.NewGuid(), "Rolename");
            await repository.AddAsync(createdRole);

            // Act
            var retrievedRole = await repository.GetRoleAsync(createdRole.Name);

            // Assert
            retrievedRole.Should().Be(createdRole);
        }
    }
}
