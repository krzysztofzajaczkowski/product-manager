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

        [Fact]
        public void MappingWarehouseProductDtoToProduct_ShouldUpdateWarehouseProperties()
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

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight,
                salesId, cost, taxPercentage, netPrice);

            var warehouseDto = new WarehouseProductDto
            {
                Sku = sku,
                Id = warehouseId,
                Stock = newStock,
                Weight = newWeight
            };

            _mapper.Map(warehouseDto, product);

            product.WarehouseProduct.Stock.Should().Be(newStock);
            product.WarehouseProduct.Weight.Should().Be(newWeight);
        }

        [Fact]
        public void MappingCatalogProductDtoToProduct_ShouldUpdateCatalogProperties()
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

            var product = new Product(catalogId, sku, productName, description, warehouseId, stock, weight,
                salesId, cost, taxPercentage, netPrice);

            var catalogProductDto = new CatalogProductDto
            {
                Sku = sku,
                Id = warehouseId,
                Name = newProductName,
                Description = newDescription
            };

            _mapper.Map(catalogProductDto, product);

            product.CatalogProduct.Name.Should().Be(newProductName);
            product.CatalogProduct.Description.Should().Be(newDescription);
        }

        [Fact]
        public void MappingSalesProductDtoToProduct_ShouldUpdateCatalogProperties()
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
            var salesProductDto = new SalesProductDto
            {
                Id = warehouseId,
                Sku = sku,
                Cost = newCost,
                TaxPercentage = newTaxPercentage,
                NetPrice = newNetPrice
            };

            _mapper.Map(salesProductDto, product);

            product.SalesProduct.Cost.Should().Be(newCost);
            product.SalesProduct.TaxPercentage.Should().Be(newTaxPercentage);
            product.SalesProduct.NetPrice.Should().Be(newNetPrice);
        }

        [Fact]
        public void MappingProductToProductBlockDto_CorrespondingPropertiesShouldBeEqual()
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
            var productBlockDto = new ProductBlockDto
            {
                Name = productName,
                Sku = sku,
                NetPrice = netPrice,
                Stock = stock
            };

            // Act
            var mappedProductDto = _mapper.Map<ProductBlockDto>(product);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(productBlockDto);
        }

        [Fact]
        public void MappingProductDtoToProductBlockDto_CorrespondingPropertiesShouldBeEqual()
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
            var productBlockDto = new ProductBlockDto
            {
                Name = productName,
                Sku = sku,
                NetPrice = netPrice,
                Stock = stock
            };

            // Act
            var mappedProductDto = _mapper.Map<ProductBlockDto>(productDto);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(productBlockDto);
        }

        [Fact]
        public void MappingCreateProductDtoToProductDto_CorrespondingPropertiesShouldBeEqual()
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

            var createProductDto = new CreateProductDto
            {
                Sku = sku,
                Name = productName,
                Description = description
            };
            var productDto = new ProductDto
            {
                Name = productName,
                Sku = sku,
                Cost = 1,
                NetPrice = 2,
                TaxPercentage = 0,
                Weight = 0,
                Description = description,
                Stock = 0,
            };

            // Act
            var mappedProductDto = _mapper.Map<ProductDto>(createProductDto);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public void MappingUpdateCatalogProductDtoToCatalogProductDto_CorrespondingPropertiesShouldBeEqual()
        {
            // Arrange
            var sku = "123";
            var productName = "product name";
            var description = "desc";
            var id = Guid.NewGuid();
            var stock = 12;
            var weight = 2.5;
            var cost = 10;
            var taxPercentage = 23;
            var netPrice = 15;

            var updateCatalogProductDto = new UpdateCatalogProductDto
            {
                Id = id,
                Sku = sku,
                Name = productName,
                Description = description
            };
            var catalogProductDto = new CatalogProductDto
            {
                Id = id,
                Sku = sku,
                Name = productName,
                Description = description
            };

            // Act
            var mappedProductDto = _mapper.Map<CatalogProductDto>(updateCatalogProductDto);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(catalogProductDto);
        }

        [Fact]
        public void MappingUpdateWarehouseProductDtoToWarehouseProductDto_CorrespondingPropertiesShouldBeEqual()
        {
            // Arrange
            var sku = "123";
            var id = Guid.NewGuid();
            var stock = 12;
            var weight = 2.5;

            var updateWarehouseProductDto = new UpdateWarehouseProductDto
            {
                Id = id,
                Sku = sku,
                Stock = stock,
                Weight = weight
            };
            var warehouseProductDto = new WarehouseProductDto
            {
                Id = id,
                Sku = sku,
                Stock = stock,
                Weight = weight
            };

            // Act
            var mappedProductDto = _mapper.Map<WarehouseProductDto>(updateWarehouseProductDto);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(warehouseProductDto);
        }

        [Fact]
        public void MappingUpdateSalesProductDtoToSalesProductDto_CorrespondingPropertiesShouldBeEqual()
        {
            // Arrange
            var sku = "123";
            var id = Guid.NewGuid();
            var cost = 10;
            var netPrice = 15;
            var taxPercentage = 23;

            var updateSalesProductDto = new UpdateSalesProductDto
            {
                Id = id,
                Sku = sku,
                NetPrice = netPrice,
                TaxPercentage = taxPercentage,
                Cost = cost
            };
            var salesProductDto = new SalesProductDto
            {
                Id = id,
                Sku = sku,
                NetPrice = netPrice,
                TaxPercentage = taxPercentage,
                Cost = cost
            };

            // Act
            var mappedProductDto = _mapper.Map<SalesProductDto>(updateSalesProductDto);

            // Assert
            mappedProductDto.Should().BeEquivalentTo(salesProductDto);
        }
    }
}
