using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Core.Domain;
using ProductManager.Core.Domain.ValueObjects;
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
                var salesProduct = new SalesProduct(Guid.NewGuid(), new StockKeepingUnit(sku),
                    new Price(cost, netPrice, taxPercentage));
                salesProduct = new SalesProduct(new StockKeepingUnit(sku),
                    new Price(cost, netPrice, taxPercentage));
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
                var salesProduct = new SalesProduct(new StockKeepingUnit(sku),
                    new Price(cost, netPrice, taxPercentage));
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
                var salesProduct = new SalesProduct(new StockKeepingUnit(sku),
                    new Price(cost, netPrice, taxPercentage));
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
                var salesProduct = new SalesProduct(new StockKeepingUnit(sku),
                    new Price(cost, netPrice, taxPercentage));
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
                var salesProduct = new SalesProduct(new StockKeepingUnit(sku),
                    new Price(cost, netPrice, taxPercentage));
            };

            // Assert
            action.Should().Throw<InvalidNetPriceException>();
        }

    }
}
