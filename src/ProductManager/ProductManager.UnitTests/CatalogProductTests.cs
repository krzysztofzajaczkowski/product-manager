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
    public class CatalogProductTests
    {
        [Theory]
        [InlineData("123", "product name", "Placeholder product description")]
        public void CreatingNewCatalogProduct_WithValidProperties_ShouldNotThrowAnyExceptions(string sku, string name, string description)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var catalogProduct = new CatalogProduct(Guid.NewGuid(), new StockKeepingUnit(sku), new ProductName(name), new ProductDescription(description));
                catalogProduct = new CatalogProduct(new StockKeepingUnit(sku), new ProductName(name), new ProductDescription(description));
            };

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData("", "product name", "Placeholder product description")]
        [InlineData(" ", "product name", "Placeholder product description")]
        [InlineData("#$", "product name", "Placeholder product description")]
        public void CreatingNewCatalogProduct_WithInvalidSku_ThrowsInvalidSkuException(string sku, string name, string description)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var catalogProduct = new CatalogProduct(new StockKeepingUnit(sku), new ProductName(name), new ProductDescription(description));
            };

            // Assert
            action.Should().Throw<InvalidSkuException>();
        }

        [Theory]
        [InlineData("123", "", "Placeholder product description")]
        [InlineData("123", " ", "Placeholder product description")]
        [InlineData("123", "#$", "Placeholder product description")]
        public void CreatingNewCatalogProduct_WithInvalidName_ThrowsInvalidProductNameException(string sku, string name, string description)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var catalogProduct = new CatalogProduct(new StockKeepingUnit(sku), new ProductName(name), new ProductDescription(description));
            };

            // Assert
            action.Should().Throw<InvalidProductNameException>();
        }

        [Theory]
        [InlineData("123", "product name", "")]
        [InlineData("123", "product name", " ")]
        [InlineData("123", "product name", "#$")]
        public void CreatingNewCatalogProduct_WithInvalidDescription_ThrowsInvalidDescriptionException(string sku, string name, string description)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var catalogProduct = new CatalogProduct(new StockKeepingUnit(sku), new ProductName(name), new ProductDescription(description));
            };

            // Assert
            action.Should().Throw<InvalidDescriptionException>();
        }
    }
}
