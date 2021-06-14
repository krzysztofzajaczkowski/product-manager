using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Configuration;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;
using Xunit;

namespace ProductManager.UnitTests
{
    public class ProductServiceTests
    {
        private readonly IMapper _mapper;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GettingAllProducts_WhenNoProducts_ShouldReturnEmptyList()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);

            repository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var retrievedProductDtos = await sut.GetAllAsync();

            // Assert
            retrievedProductDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GettingAllProducts_WhenProductsExist_ReturnedListShouldNotBeEmpty()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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

            repository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Product>
            {
                product
            });

            // Act
            var retrievedProductDtos = await sut.GetAllAsync();

            // Assert
            retrievedProductDtos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GettingAllProducts_WhenProductsExist_ShouldReturnListWithProductProjections()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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

            repository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Product>
            {
                product
            });

            // Act
            var retrievedProductDtos = await sut.GetAllAsync();

            // Assert
            retrievedProductDtos.First().Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public void GettingProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
            var sku = "123";

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync((Product)null);

            // Act
            Func<Task> action = async () =>
            {
                var retrievedProductDto = await sut.GetAsync(sku);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GettingProduct_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            retrievedProductDto.Should().BeEquivalentTo(productDto);
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddingProduct_WhenProductDoesNotExist_AddAsyncShouldBecalledOnce()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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

            // Act
            await sut.AddAsync(productDto);

            // Assert
            repository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task AddingProduct_WhenProductDoesNotExist_ProductShouldBeAdded()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
            retrievedProductDto.Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public void AddingProduct_WhenProductExists_ShouldThrowProductAlreadyExistsException()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            Func<Task> action = async () =>
            {
                await sut.AddAsync(productDto);
            };

            // Assert
            action.Should().Throw<ProductAlreadyExistsException>();
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void DeletingProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var sku = "123";
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync((Product)null);

            // Act
            Func<Task> action = async () =>
            {
                await sut.DeleteAsync(sku);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeletingProduct_WhenProductExists_ShouldCallRepositoryDeleteAsyncOnce()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);

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

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.DeleteAsync(sku);

            // Assert
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
            repository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatingWarehouseProduct_WhenProductExists_ShouldUpdateCorrespondingProperties()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            var warehouseProductDto = new WarehouseProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Stock = newStock,
                Weight = newWeight
            };

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.UpdateWarehouseAsync(warehouseProductDto);
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Exactly(2));
            retrievedProductDto.Stock.Should().Be(warehouseProductDto.Stock);
            retrievedProductDto.Weight.Should().Be(warehouseProductDto.Weight);
        }

        [Fact]
        public void UpdatingWarehouseProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
                await sut.UpdateWarehouseAsync(warehouseProductDto);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatingWarehouseProduct_ShouldCallRepositoryUpdateAsyncOnce()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            var warehouseProductDto = new WarehouseProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Stock = newStock,
                Weight = newWeight
            };

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.UpdateWarehouseAsync(warehouseProductDto);
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdatingCatalogProduct_WhenProductExists_ShouldUpdateCorrespondingProperties()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            var catalogProductDto = new CatalogProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Name = newProductName,
                Description = newDescription
            };

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.UpdateCatalogAsync(catalogProductDto);
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Exactly(2));
            retrievedProductDto.Name.Should().Be(catalogProductDto.Name);
            retrievedProductDto.Description.Should().Be(catalogProductDto.Description);
        }

        [Fact]
        public void UpdatingCatalogProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
                await sut.UpdateCatalogAsync(catalogProductDto);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatingCatalogProduct_ShouldCallRepositoryUpdateAsyncOnce()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            var catalogProductDto = new CatalogProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Name = newProductName,
                Description = newDescription
            };

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.UpdateCatalogAsync(catalogProductDto);
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdatingSalesProduct_WhenProductExists_ShouldUpdateCorrespondingProperties()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            var salesProductDto = new SalesProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Cost = newCost,
                TaxPercentage = newTaxPercentage,
                NetPrice = newNetPrice
            };

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.UpdateSalesAsync(salesProductDto);
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Exactly(2));
            retrievedProductDto.Cost.Should().Be(salesProductDto.Cost);
            retrievedProductDto.TaxPercentage.Should().Be(salesProductDto.TaxPercentage);
            retrievedProductDto.NetPrice.Should().Be(salesProductDto.NetPrice);
        }

        [Fact]
        public void UpdatingSalesProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
                await sut.UpdateSalesAsync(salesProductDto);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
            repository.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatingSalesProduct_ShouldCallRepositoryUpdateAsyncOnce()
        {
            // Arrange
            var repository = new Mock<IProductRepository>();
            var sut = new ProductService(_mapper, repository.Object);
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
            var salesProductDto = new SalesProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Cost = newCost,
                TaxPercentage = newTaxPercentage,
                NetPrice = newNetPrice
            };

            repository.Setup(x => x.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

            // Act
            await sut.UpdateSalesAsync(salesProductDto);
            var retrievedProductDto = await sut.GetAsync(sku);

            // Assert
            repository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
