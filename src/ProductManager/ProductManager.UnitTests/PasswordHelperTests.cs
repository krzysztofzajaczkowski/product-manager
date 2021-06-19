using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Infrastructure.Helper;
using Xunit;

namespace ProductManager.UnitTests
{
    public class PasswordHelperTests
    {
        [Fact]
        public void CalculatedHash_ShouldNotEqualToOriginalPassword()
        {
            // Arrange
            var password = "secret";

            // Act
            var hashedPassword = PasswordHelper.CalculateHash(password);

            // Assert
            hashedPassword.Should().NotBe(password);
        }

        [Fact]
        public void CheckMatch_ShouldReturnTrueWhenHashedPasswordEqualsInputPassword()
        {
            // Arrange
            var originalPassword = "secret";
            var inputPassword = originalPassword;
            var hashedPassword = PasswordHelper.CalculateHash(originalPassword);

            // Act
            var isMatched = PasswordHelper.CheckMatch(hashedPassword, inputPassword);

            // Assert
            isMatched.Should().BeTrue();
        }

        [Fact]
        public void CheckMatch_ShouldReturnFalseWhenHashedPasswordDoesNotEqualInputPassword()
        {
            // Arrange
            var originalPassword = "secret";
            var inputPassword = $"{originalPassword}1";
            var hashedPassword = PasswordHelper.CalculateHash(originalPassword);

            // Act
            var isMatched = PasswordHelper.CheckMatch(hashedPassword, inputPassword);

            // Assert
            isMatched.Should().BeFalse();
        }
    }
}
