using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using ProductManager.Infrastructure.Database;
using ProductManager.Infrastructure.Repositories;
using ProductManager.Infrastructure.Services;
using ProductManager.Infrastructure.Settings;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class SQLiteUserRepositoryTests : IDisposable
    {
        private readonly string _dbFileName;
        private readonly string _connectionString;

        public SQLiteUserRepositoryTests()
        {
            _dbFileName = "database.db";
            _connectionString = $"DataSource={_dbFileName};BinaryGUID=False;";
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_AdminAccountShouldExist()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var account = await sut.GetUserAsync("admin@admin.com");

            // Assert
            account.Should().NotBeNull();
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_CatalogManagerRoleShouldExist()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var role = await sut.GetRoleAsync("CatalogManager");

            // Assert
            role.Should().NotBeNull();
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_WarehouseManagerRoleShouldExist()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var role = await sut.GetRoleAsync("WarehouseManager");

            // Assert
            role.Should().NotBeNull();
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_SalesManagerRoleShouldExist()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var role = await sut.GetRoleAsync("SalesManager");

            // Assert
            role.Should().NotBeNull();
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_AdminAccountShouldHaveCatalogManagerRole()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var account = await sut.GetUserAsync("admin@admin.com");

            // Assert
            account.Roles.Should().Contain(r => r.Name == "CatalogManager");
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_AdminAccountShouldHaveWarehouseManagerRole()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var account = await sut.GetUserAsync("admin@admin.com");

            // Assert
            account.Roles.Should().Contain(r => r.Name == "WarehouseManager");
        }

        [Fact]
        public async Task WhenUsingDatabaseInitializer_AdminAccountShouldHaveSalesManagerRole()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteUserRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var account = await sut.GetUserAsync("admin@admin.com");

            // Assert
            account.Roles.Should().Contain(r => r.Name == "SalesManager");
        }

        public void Dispose()
        {
            if (File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
        }
    }
}
