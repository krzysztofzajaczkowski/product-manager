using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Specialized;
using ProductManager.Core.Domain;
using ProductManager.Core.Domain.ValueObjects;
using ProductManager.Infrastructure.Database;
using ProductManager.Infrastructure.Repositories;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class SQLiteProductRepositoryTests : IDisposable
    {
        private readonly string _dbFileName;
        private readonly string _connectionString;

        public SQLiteProductRepositoryTests()
        {
            _dbFileName = "database.db";
            _connectionString = $"DataSource={_dbFileName};BinaryGUID=False;";
            if (File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenUsingDatabaseInitializer_ProductsShouldExist()
        {
            // Arrange
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteProductRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            // Act
            var products = await sut.GetAllAsync();

            // Assert
            products.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AddAsync_WhenUsingDatabaseInitializer_ShouldCreateProductInDatabase()
        {
            // Arrange
            var sku = "123";
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteProductRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();

            Func<Task> action = async () =>
            {
                // Act
                await sut.AddAsync(new Product(Guid.NewGuid(), sku, "Prod123A", "Desc1", Guid.NewGuid(), 0, 2.5,
                    Guid.NewGuid(), 10, 23, 15));
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task GetAsync_WhenUsingDatabaseInitializer_ShouldReturnProductFromDatabase()
        {
            // Arrange
            var sku = "123";
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteProductRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();
            await sut.AddAsync(new Product(Guid.NewGuid(), sku, "Prod123A", "Desc1", Guid.NewGuid(), 0, 2.5, Guid.NewGuid(), 10, 23, 15));

            // Act
            var product = await sut.GetProductAsync(sku);

            // Assert
            product.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenUsingDatabaseInitializer_ShouldUpdateProductInDatabase()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var name = "Prod123A";
            var newName = "Prod456B";
            var description = "Desc1";
            var stock = 0;
            var newStock = 15;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 20;
            var newNetPrice = 30;
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteProductRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();
            await sut.AddAsync(new Product(catalogId, sku, name, description, warehouseId, stock, weight, salesId, cost, taxPercentage, netPrice));

            // Act
            Func<Task> action = async () =>
            {
                await sut.UpdateAsync(new Product(catalogId, sku, newName, description, warehouseId, newStock, weight,
                    warehouseId, cost, taxPercentage, newNetPrice));
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task GetAsync_AfterUpdateWhenUsingDatabaseInitializer_ShouldReturnUpdateProductFromDatabase()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var name = "Prod123A";
            var newName = "Prod456B";
            var description = "Desc1";
            var stock = 0;
            var newStock = 15;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 20;
            var newNetPrice = 30;
            var dbConnectionFactory = new SQLiteConnectionFactory(_connectionString);
            var dbInitializer = new DatabaseInitializier(dbConnectionFactory);
            var sut = new SQLiteProductRepository(dbConnectionFactory);

            await dbInitializer.SeedDatabaseAsync();
            await sut.AddAsync(new Product(catalogId, sku, name, description, warehouseId, stock, weight, salesId, cost, taxPercentage, netPrice));

            // Act
            await sut.UpdateAsync(new Product(catalogId, sku, newName, description, warehouseId, newStock, weight,
                    warehouseId, cost, taxPercentage, newNetPrice));
            var product = await sut.GetProductAsync(sku);


            // Assert
            product.CatalogProduct.Name.Should().BeEquivalentTo(new ProductName(newName));
            product.WarehouseProduct.Stock.Should().BeEquivalentTo(new ProductStock(newStock));
            product.SalesProduct.Price.NetPrice.Should().Be(newNetPrice);
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
