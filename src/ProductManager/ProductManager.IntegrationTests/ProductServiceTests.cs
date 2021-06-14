using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class ProductServiceTests : TestServerBase, IDisposable
    {
        private readonly IProductService _sut;
        private readonly TestServer _server;

        public ProductServiceTests()
        {
            _server = BuildTestServer();
            using var scope = _server.Services.CreateScope();
            _sut = scope.ServiceProvider.GetRequiredService<IProductService>();
        }

        [Fact]
        public async Task GettingAllProducts_WhenNoProducts_ShouldReturnEmptyList()
        {
            // Act
            var retrievedProductDtos = await _sut.GetAllAsync();

            // Assert
            retrievedProductDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GettingAllProducts_WhenProductsExist_ReturnedListShouldNotBeEmpty()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await _sut.AddAsync(productDto);

            // Act
            var retrievedProductDtos = await _sut.GetAllAsync();

            // Assert
            retrievedProductDtos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GettingAllProducts_WhenProductsExist_ShouldReturnListWithProductProjections()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await _sut.AddAsync(productDto);

            // Act
            var retrievedProductDtos = await _sut.GetAllAsync();

            // Assert
            retrievedProductDtos.First().Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public void GettingProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var sku = "123";

            // Act
            Func<Task> action = async () =>
            {
                var retrievedProductDto = await _sut.GetAsync(sku);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task GettingProduct_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight, salesId,
                cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await _sut.AddAsync(productDto);

            // Act
            var retrievedProductDto = await _sut.GetAsync(sku);

            // Assert
            retrievedProductDto.Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public async Task AddingProduct_WhenProductDoesNotExist_ProductShouldBeAdded()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight, salesId,
                cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await _sut.AddAsync(productDto);

            // Act
            var retrievedProductDto = await _sut.GetAsync(sku);

            // Assert
            retrievedProductDto.Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public async Task AddingProduct_WhenProductExists_ShouldThrowProductAlreadyExistsException()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight, salesId,
                cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await _sut.AddAsync(productDto);

            // Act
            Func<Task> action = async () =>
            {
                await _sut.AddAsync(productDto);
            };

            // Assert
            action.Should().Throw<ProductAlreadyExistsException>();
        }

        [Fact]
        public void DeletingProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var sku = "123";

            // Act
            Func<Task> action = async () =>
            {
                await _sut.DeleteAsync(sku);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task UpdatingWarehouseProduct_WhenProductExists_ShouldUpdateCorrespondingProperties()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var newStock = 14;
            var weight = 2.5;
            var newWeight = 4.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight, salesId,
                cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            var warehouseProductDto = new WarehouseProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Stock = newStock,
                Weight = newWeight
            };
            await _sut.AddAsync(productDto);

            // Act
            await _sut.UpdateWarehouseAsync(warehouseProductDto);
            var retrievedProductDto = await _sut.GetAsync(sku);

            // Assert
            retrievedProductDto.Stock.Should().Be(warehouseProductDto.Stock);
            retrievedProductDto.Weight.Should().Be(warehouseProductDto.Weight);
        }

        [Fact]
        public void UpdatingWarehouseProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var warehouseId = Guid.NewGuid();
            var sku = "123";
            var newStock = 14;
            var newWeight = 4.5;

            var warehouseProductDto = new WarehouseProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Stock = newStock,
                Weight = newWeight
            };

            // Act
            Func<Task> action = async () =>
            {
                await _sut.UpdateWarehouseAsync(warehouseProductDto);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task UpdatingCatalogProduct_WhenProductExists_ShouldUpdateCorrespondingProperties()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var newProductName = "product name 2";
            var description = "desc";
            var newDescription = "desc 2";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight, salesId,
                cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            var catalogProductDto = new CatalogProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Name = newProductName,
                Description = newDescription
            };
            await _sut.AddAsync(productDto);

            // Act
            await _sut.UpdateCatalogAsync(catalogProductDto);
            var retrievedProductDto = await _sut.GetAsync(sku);

            // Assert
            retrievedProductDto.Name.Should().Be(catalogProductDto.Name);
            retrievedProductDto.Description.Should().Be(catalogProductDto.Description);
        }

        [Fact]
        public void UpdatingCatalogProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var warehouseId = Guid.NewGuid();
            var sku = "123";
            var newProductName = "product name 2";
            var newDescription = "desc 2";

            var catalogProductDto = new CatalogProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Name = newProductName,
                Description = newDescription
            };

            // Act
            Func<Task> action = async () =>
            {
                await _sut.UpdateCatalogAsync(catalogProductDto);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task UpdatingSalesProduct_WhenProductExists_ShouldUpdateCorrespondingProperties()
        {
            // Arrange
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var newCost = 12;
            var taxPercentage = 23;
            var newTaxPercentage = 8;
            var netPrice = 15;
            var newNetPrice = 17;

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight, salesId,
                cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            var salesProductDto = new SalesProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Cost = newCost,
                TaxPercentage = newTaxPercentage,
                NetPrice = newNetPrice
            };
            await _sut.AddAsync(productDto);

            // Act
            await _sut.UpdateSalesAsync(salesProductDto);
            var retrievedProductDto = await _sut.GetAsync(sku);

            // Assert
            retrievedProductDto.Cost.Should().Be(salesProductDto.Cost);
            retrievedProductDto.TaxPercentage.Should().Be(salesProductDto.TaxPercentage);
            retrievedProductDto.NetPrice.Should().Be(salesProductDto.NetPrice);
        }

        [Fact]
        public void UpdatingSalesProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var warehouseId = Guid.NewGuid();
            var sku = "123";
            var newCost = 12;
            var newTaxPercentage = 8;
            var newNetPrice = 17;

            var salesProductDto = new SalesProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Cost = newCost,
                TaxPercentage = newTaxPercentage,
                NetPrice = newNetPrice
            };

            // Act
            Func<Task> action = async () =>
            {
                await _sut.UpdateSalesAsync(salesProductDto);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
