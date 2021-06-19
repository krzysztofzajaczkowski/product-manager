using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Infrastructure.Database;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class SQLiteConnectionFactoryTests : TestServerBase, IDisposable
    {
        private readonly TestServer _testServer;
        private readonly string _connectionString;
        private readonly string _dbFileName;

        public SQLiteConnectionFactoryTests()
        {
            _testServer = BuildTestServer();
            _connectionString = _testServer.Services.GetRequiredService<IConfiguration>().GetConnectionString("SQLite");
            _dbFileName = _connectionString.Split(";").First().Split("=").Last();
        }

        [Fact]
        public void AccessingSQLiteConnectionFactory_ConnectionStringFieldShouldEqualConfiguredConnectionString()
        {
            // Arrange
            var sut = _testServer.Services.GetRequiredService<IDbConnectionFactory>();
            var connectionStringField = sut.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "_connectionString" && x.IsPrivate);

            // Act
            var value = (string)connectionStringField.GetValue(sut);

            // Assert
            value.Should().Be(_connectionString);
        }

        [Fact]
        public async Task WhenCallingCreate_FactoryShouldReturnOpenedIDbConnection()
        {
            // Arrange
            var sut = _testServer.Services.GetRequiredService<IDbConnectionFactory>();

            // Act
            var conn = await sut.CreateAsync();
            var caughtState = conn.State;
            conn.Close();

            // Assert
            caughtState.Should().Be(ConnectionState.Open);
        }

        [Fact]
        public async Task WhenCallingCreate_FactoryShouldCreateFileWithSpecifiedPath()
        {
            // Arrange
            var sut = _testServer.Services.GetRequiredService<IDbConnectionFactory>();
            
            // Act
            var conn = await sut.CreateAsync();
            conn.Close();

            // Assert
            File.Exists(_dbFileName).Should().BeTrue();
        }

        public void Dispose()
        {
            if (File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
            _testServer.Dispose();
        }
    }
}
