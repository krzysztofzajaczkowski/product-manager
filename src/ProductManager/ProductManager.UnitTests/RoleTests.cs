using System;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using Xunit;

namespace ProductManager.UnitTests
{
    public class RoleTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("*")]
        public void CreatingNewRole_WithInvalidName_ShouldThrowInvalidRoleNameException(string name)
        {
            // Arrange
            Action action = () =>
            {
                var role = new Role(name);
            };

            // Act/Assert
            action.Should().Throw<InvalidRoleNameException>();
        }

        [Fact]
        public void CreatingNewRole_WithValidName_ShouldNotThrowAnyException()
        {
            // Arrange
            var name = "testRole";
            Action action = () =>
            {
                var role = new Role(name);
            };

            // Act/Assert
            action.Should().NotThrow();
        }

    }
}
