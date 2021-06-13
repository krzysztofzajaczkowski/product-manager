using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Infrastructure.Configuration;
using ProductManager.Infrastructure.Repositories;
using Xunit;

namespace ProductManager.UnitTests
{
    public class InMemoryProductRepositoryTests
    {
        private readonly IMapper _mapper;

        public InMemoryProductRepositoryTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task RetrievingProduct_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var sku = "123";
            var sut = new InMemoryProductRepository(_mapper);

            // Act
            var product = await sut.GetProductAsync(sku);

            // Assert
            product.Should().BeNull();
        }

        [Fact]
        public async Task AddingProduct_WhenProductDoesNotExists_ProductShouldBeAdded()
        {
            // Arrange
            var sku = "123";
            var product = new Product(Guid.NewGuid(), sku, "product name", "product desc", Guid.NewGuid(), 1, 1.5,
                Guid.NewGuid(), 10, 23, 15);
            var sut = new InMemoryProductRepository(_mapper);
            await sut.AddAsync(product);

            // Act
            var retrievedProduct = await sut.GetProductAsync(sku);

            // Assert
            retrievedProduct.Should().NotBeNull();
        }

        [Fact]
        public async Task RetrievingProduct_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var sku = "123";
            var product = new Product(Guid.NewGuid(), sku, "product name", "product desc", Guid.NewGuid(), 1, 1.5,
                Guid.NewGuid(), 10, 23, 15);
            var sut = new InMemoryProductRepository(_mapper);
            await sut.AddAsync(product);

            // Act
            var retrievedProduct = await sut.GetProductAsync(sku);

            // Assert
            retrievedProduct.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task DeletingProduct_WhenProductExists_ShouldDeleteProduct()
        {
            // Arrange
            var sku = "123";
            var product = new Product(Guid.NewGuid(), sku, "product name", "product desc", Guid.NewGuid(), 1, 1.5,
                Guid.NewGuid(), 10, 23, 15);
            var sut = new InMemoryProductRepository(_mapper);
            await sut.AddAsync(product);

            // Act
            await sut.DeleteAsync(sku);
            var retrievedProduct = await sut.GetProductAsync(sku);

            // Assert
            retrievedProduct.Should().BeNull();
        }

        [Fact]
        public void DeletingProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var sku = "123";
            var sut = new InMemoryProductRepository(_mapper);

            // Act
            Func<Task> action = async () =>
            {
                await sut.DeleteAsync(sku);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public void UpdatingProduct_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var sku = "123";
            var product = new Product(Guid.NewGuid(), sku, "product name", "product desc", Guid.NewGuid(), 1, 1.5,
                Guid.NewGuid(), 10, 23, 15);
            var sut = new InMemoryProductRepository(_mapper);

            // Act
            Func<Task> action = async () =>
            {
                await sut.UpdateAsync(product);
            };

            // Assert
            action.Should().Throw<ProductNotFoundException>();
        }

        [Fact]
        public async Task UpdatingProduct_WhenProductExists_ShouldUpdateProductProperties()
        {
            // Arrange
            var sku = "123";
            var catalogId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var salesId = Guid.NewGuid();
            var product = new Product(catalogId, sku, "product name", "product desc", warehouseId, 1, 1.5,
                salesId, 10, 23, 15);
            var newProduct = new Product(catalogId, sku, "product name 2", "product desc 2", warehouseId, 2, 2.5,
                salesId, 11, 24, 16);
            var sut = new InMemoryProductRepository(_mapper);

            await sut.AddAsync(product);

            // Act
            await sut.UpdateAsync(newProduct);
            var retrievedProduct = await sut.GetProductAsync(newProduct.CatalogProduct.Sku);

            // Assert
            retrievedProduct.Should().BeEquivalentTo(newProduct);
        }
    }
}
