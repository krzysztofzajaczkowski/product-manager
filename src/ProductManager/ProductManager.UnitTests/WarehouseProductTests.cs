using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Domain.ValueObjects;
using ProductManager.Core.Exceptions;
using Xunit;

namespace ProductManager.UnitTests
{
    public class WarehouseProductTests
    {
        [Theory]
        [InlineData("123", 12, 4.5)]
        public void CreatingNewWarehouseProduct_WithValidProperties_ShouldNotThrowAnyExceptions(string sku, int stock, double weight)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                //var warehouseProduct = new WarehouseProduct(Guid.NewGuid(), sku, stock, weight);
                var warehouseProduct = new WarehouseProduct(Guid.NewGuid(), new StockKeepingUnit(sku), new ProductStock(stock), new ProductWeight(weight));
                warehouseProduct = new WarehouseProduct(new StockKeepingUnit(sku), new ProductStock(stock), new ProductWeight(weight));
            };

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("", 12, 4.5)]
        [InlineData("#$", 12, 4.5)]
        public void CreatingNewWarehouseProduct_WithInvalidSku_ThrowsInvalidSkuException(string sku, int stock, double weight)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var warehouseProduct = new WarehouseProduct(new StockKeepingUnit(sku), new ProductStock(stock), new ProductWeight(weight));
            };

            // Assert
            action.Should().Throw<InvalidSkuException>();
        }

        [Theory]
        [InlineData("123", -1, 4.5)]
        public void CreatingNewWarehouseProduct_WithInvalidStock_ThrowsInvalidStockException(string sku, int stock, double weight)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var warehouseProduct = new WarehouseProduct(new StockKeepingUnit(sku), new ProductStock(stock), new ProductWeight(weight));
            };

            // Assert
            action.Should().Throw<InvalidStockException>();
        }

        [Theory]
        [InlineData("123", 1, -1)]
        [InlineData("123", 1, -0.1)]
        public void CreatingNewWarehouseProduct_WithInvalidWeight_ThrowsInvalidWeightException(string sku, int stock, double weight)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var warehouseProduct = new WarehouseProduct(new StockKeepingUnit(sku), new ProductStock(stock), new ProductWeight(weight));
            };

            // Assert
            action.Should().Throw<InvalidWeightException>();
        }
    }
}
