using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using Xunit;

namespace ProductManager.UnitTests
{
    public class SalesProductTests
    {
        [Theory]
        [InlineData("123", 12, 4.5, 30)]
        public void CreatingNewSalesProduct_WithValidProperties_ShouldNotThrowAnyExceptions(string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct(Guid.NewGuid(), sku, cost, taxPercentage, netPrice);
                salesProduct = new SalesProduct(sku, cost, taxPercentage, netPrice);
            };

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("", 12, 4.5, 30)]
        [InlineData(" ", 12, 4.5, 30)]
        [InlineData("$#", 12, 4.5, 30)]
        public void CreatingNewSalesProduct_WithInvalidSku_ThrowsInvalidSkuException(string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct(sku, cost, taxPercentage, netPrice);
            };

            // Assert
            action.Should().Throw<InvalidSkuException>();
        }

        [Theory]
        [InlineData("123", 0, 4.5, 30)]
        [InlineData("123", -2, 4.5, 30)]
        public void CreatingNewSalesProduct_WithInvalidCost_ThrowsInvalidCostException(string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct(sku, cost, taxPercentage, netPrice);
            };

            // Assert
            action.Should().Throw<InvalidCostException>();
        }

        [Theory]
        [InlineData("123", 5, 4.5, -2)]
        [InlineData("123", 5, 4.5, 0)]
        public void CreatingNewSalesProduct_WithInvalidNetPrice_ThrowsInvalidNetPriceException(string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct(sku, cost, taxPercentage, netPrice);
            };

            // Assert
            action.Should().Throw<InvalidNetPriceException>();
        }

        [Theory]
        [InlineData("123", 5, 4.5, 1)]
        [InlineData("123", 5, 4.5, 2)]
        public void CreatingNewSalesProduct_WhenCostGreaterThanOrEqualToNetPrice_ThrowsInvalidNetPriceException(string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct(sku, cost, taxPercentage, netPrice);
            };

            // Assert
            action.Should().Throw<InvalidNetPriceException>();
        }


        [Fact]
        public void ChangingSalesProductSku_WithValidSku_ShouldUpdateSkuProperty()
        {
            // Arrange
            var salesProduct = new SalesProduct("123", 10, 23, 11);
            var newSku = "456";

            // Act
            salesProduct.SetSku(newSku);

            // Arrange
            salesProduct.Sku.Should().Be(newSku);
        }

        [Fact]
        public void ChangingSalesProductCost_WithValidCost_ShouldUpdateCostProperty()
        {
            // Arrange
            var salesProduct = new SalesProduct("123", 8, 23, 11);
            var newCost = 10;

            // Act
            salesProduct.SetCost(newCost);

            // Arrange
            salesProduct.Cost.Should().Be(newCost);
        }

        [Theory]
        [InlineData(10, 9, 10)]
        [InlineData(10, 9, 11)]
        public void ChangingSalesProductCost_WithInvalidCost_ShouldThrowInvalidCostException(decimal netPrice, decimal oldCost, decimal newCost)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct("123", oldCost, 23, netPrice);
                salesProduct.SetCost(newCost);
            };

            // Arrange
            action.Should().Throw<InvalidCostException>();
        }

        [Theory]
        [InlineData(10, 11, 10)]
        [InlineData(10, 11, 9)]
        public void ChangingSalesProductNetPrice_WithInvalidNetPrice_ShouldThrowInvalidNetPriceException(decimal cost, decimal oldNetPrice, decimal newNetPrice)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct("123", cost, 23, oldNetPrice);
                salesProduct.SetNetPrice(newNetPrice);
            };

            // Arrange
            action.Should().Throw<InvalidNetPriceException>();
        }

        [Fact]
        public void ChangingSalesProductCostAndNetPrice_WithInvalidCost_ShouldThrowInvalidCostException()
        {
            // Arrange
            var newCost = 0;
            var newNetPrice = 11;
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct("123", 10, 23, 11);
                salesProduct.SetCostAndNetPrice(newCost, newNetPrice);
            };

            // Arrange
            action.Should().Throw<InvalidCostException>();
        }

        [Fact]
        public void ChangingSalesProductCostAndNetPrice_WithInvalidCostAndNetPrice_ShouldThrowInvalidNetPriceException()
        {
            // Arrange
            var newCost = 11;
            var newNetPrice = 11;
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct("123", 10, 23, 11);
                salesProduct.SetCostAndNetPrice(newCost, newNetPrice);
            };

            // Arrange
            action.Should().Throw<InvalidNetPriceException>();
        }

        [Fact]
        public void ChangingSalesProductCostAndNetPrice_WithValidCostAndNetPrice_PropertiesShouldBeUpdated()
        {
            // Arrange
            var newCost = 20;
            var newNetPrice = 25;

            // Act
            var salesProduct = new SalesProduct("123", 10, 23, 11);
            salesProduct.SetCostAndNetPrice(newCost, newNetPrice);

            // Arrange
            salesProduct.NetPrice.Should().Be(newNetPrice);
            salesProduct.Cost.Should().Be(newCost);
        }

        [Fact]
        public void ChangingSalesProductCostAndNetPrice_WithInvalidNetPrice_ShouldThrowInvalidNetPriceException()
        {
            // Arrange
            var newCost = 10;
            var newNetPrice = 0;
            Action action = () =>
            {
                // Act
                var salesProduct = new SalesProduct("123", 10, 23, 11);
                salesProduct.SetCostAndNetPrice(newCost, newNetPrice);
            };

            // Arrange
            action.Should().Throw<InvalidNetPriceException>();
        }

        [Fact]
        public void ChangingSalesProductNetPrice_WithValidNetPrice_ShouldUpdateNetPriceProperty()
        {
            // Arrange
            var salesProduct = new SalesProduct("123", 8, 23, 11);
            var newNetPrice = 10;

            // Act
            salesProduct.SetNetPrice(newNetPrice);

            // Arrange
            salesProduct.NetPrice.Should().Be(newNetPrice);
        }


    }
}
