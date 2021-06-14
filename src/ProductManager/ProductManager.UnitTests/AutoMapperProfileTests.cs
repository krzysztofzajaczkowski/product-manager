using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Infrastructure.Configuration;
using ProductManager.Infrastructure.DTO;
using Xunit;

namespace ProductManager.UnitTests
{
    public class AutoMapperProfileTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfileTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MappingProductToProductDto_CorrespondingPropertiesShouldBeEqual()
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

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight,
                salesId, cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                Name = productName,
                Sku = sku,
                SalesId = salesId,
                Cost = cost,
                NetPrice = netPrice,
                TaxPercentage = taxPercentage,
                Weight = weight,
                Description = description,
                WarehouseId = warehouseId,
                Stock = stock,
                CatalogId = catalogId
            };

            // Act
            var mappedProductDto = _mapper.Map<ProductDto>(product);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public void MappingProductDtoToProduct_CorrespondingPropertiesShouldBeEqual()
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

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight,
                salesId, cost, taxPercentage, netPrice);
            var productDto = new ProductDto
            {
                Name = productName,
                Sku = sku,
                SalesId = salesId,
                Cost = cost,
                NetPrice = netPrice,
                TaxPercentage = taxPercentage,
                Weight = weight,
                Description = description,
                WarehouseId = warehouseId,
                Stock = stock,
                CatalogId = catalogId
            };

            // Act
            var mappedProduct = _mapper.Map<Product>(product);

            // Assert
            mappedProduct.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void MappingProductDtoToProduct_WhenGuidsEmpty_ShouldGenerateGuids()
        {
            // Arrange
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
                Name = productName,
                Sku = sku,
                Cost = cost,
                NetPrice = netPrice,
                TaxPercentage = taxPercentage,
                Weight = weight,
                Description = description,
                Stock = stock,
            };

            // Act
            var mappedProduct = _mapper.Map<Product>(productDto);

            // Assert
            mappedProduct.CatalogProduct.Id.Should().NotBe(Guid.Empty);
            mappedProduct.WarehouseProduct.Id.Should().NotBe(Guid.Empty);
            mappedProduct.SalesProduct.Id.Should().NotBe(Guid.Empty);
        }
    }
}
