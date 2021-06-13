using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Core.Domain;
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
                var warehouseProduct = new WarehouseProduct(Guid.NewGuid(), sku, stock, weight);
                warehouseProduct = new WarehouseProduct(sku, stock, weight);
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
                var warehouseProduct = new WarehouseProduct(sku, stock, weight);
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
                var warehouseProduct = new WarehouseProduct(sku, stock, weight);
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
                var warehouseProduct = new WarehouseProduct(sku, stock, weight);
            };

            // Assert
            action.Should().Throw<InvalidWeightException>();
        }

        [Fact]
        public void ChangingWarehouseProductSku_WithValidSku_ShouldUpdateSkuProperty()
        {
            // Arrange
            var warehouseProduct = new WarehouseProduct("123", 10, 2.2);
            var newSku = "456";

            // Act
            warehouseProduct.SetSku(newSku);

            // Arrange
            warehouseProduct.Sku.Should().Be(newSku);
        }

        [Fact]
        public void ChangingWarehouseProductStock_WithValidStock_ShouldUpdateStockProperty()
        {
            // Arrange
            var warehouseProduct = new WarehouseProduct("123", 10, 2.2);
            var newStock = 15;

            // Act
            warehouseProduct.SetStock(newStock);

            // Arrange
            warehouseProduct.Stock.Should().Be(newStock);
        }

        [Fact]
        public void ChangingWarehouseProductWeight_WithValidWeight_ShouldUpdateWeightProperty()
        {
            // Arrange
            var warehouseProduct = new WarehouseProduct("123", 10, 2.2);
            var newWeight = 5;

            // Act
            warehouseProduct.SetWeight(newWeight);

            // Arrange
            warehouseProduct.Weight.Should().Be(newWeight);
        }
    }
}
