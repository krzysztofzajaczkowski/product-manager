using System;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using Xunit;

namespace ProductManager.UnitTests
{
    public class UserTests
    {

        [Theory]
        [InlineData("test", "test@email.com", "pass")]
        public void CreatingNewUser_WithValidProperties_ShouldNotThrowAnyExceptions(string name, string email, string password)
        {
            // Arrange
            Action action = () =>
            {
                var user = new User(name, email, password);
            };

            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("", "email@email.com", "pass")]
        [InlineData(" ", "email@email.com", "pass")]
        [InlineData("test ", "email@email.com", "pass")]
        [InlineData("te st", "email@email.com", "pass")]
        [InlineData(" test", "email@email.com", "pass")]
        [InlineData("/", "email@email.com", "pass")]
        [InlineData("/test", "email@email.com", "pass")]
        [InlineData("test*", "email@email.com", "pass")]
        public void CreatingNewUser_WithInvalidName_ThrowsInvalidUsernameException(string name, string email, string password)
        {
            // Arrange
            Action action = () =>
            {
                var user = new User(name, email, password);
            };

            // Act/Assert
            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("test", "", "pass")]
        [InlineData("test", " ", "pass")]
        [InlineData("test", "email @email.com", "pass")]
        [InlineData("test", "email/ @email.com", "pass")]
        [InlineData("test", "email/* @email.com", "pass")]
        public void CreatingNewUser_WithInvalidEmail_ThrowsInvalidEmailException(string name, string email, string password)
        {
            // Arrange
            Action action = () =>
            {
                var user = new User(name, email, password);
            };

            // Act/Assert
            action.Should().Throw<InvalidEmailException>();
        }

        [Theory]
        [InlineData("test", "email@email.com", "")]
        [InlineData("test", "email@email.com", " ")]
        [InlineData("test", "email@email.com", "*")]
        [InlineData("test", "email@email.com", "/")]
        public void CreatingNewUser_WithInvalidPassword_ThrowsInvalidPasswordException(string name, string email, string password)
        {
            // Arrange
            Action action = () =>
            {
                var user = new User(name, email, password);
            };

            // Act/Assert
            action.Should().Throw<InvalidPasswordException>();
        }
    }
}
