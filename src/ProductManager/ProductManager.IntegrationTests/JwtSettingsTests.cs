using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductManager.Infrastructure.Services;
using ProductManager.Infrastructure.Settings;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class JwtSettingsTests : TestServerBase
    {
        [Fact]
        public void WhenRetrievingServiceFromScope_WhenServiceIsRegistered_ShouldReturnConfiguredOptions()
        {
            // Arrange
            var server = BuildTestServer();
            using var scope = server.Services.CreateScope();

            // Act
            var jwtHandler = scope.ServiceProvider.GetService<IOptions<JwtSettings>>();

            // Assert
            jwtHandler.Value.Should().BeEquivalentTo(_jwtSettings);
        }
    }
}
