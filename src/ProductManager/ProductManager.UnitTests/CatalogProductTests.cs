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
                var catalogProduct = new CatalogProduct(Guid.NewGuid(), sku, name, description);
                catalogProduct = new CatalogProduct(sku, name, description);
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
                var catalogProduct = new CatalogProduct(sku, name, description);
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
                var catalogProduct = new CatalogProduct(sku, name, description);
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
                var catalogProduct = new CatalogProduct(sku, name, description);
            };

            // Assert
            action.Should().Throw<InvalidDescriptionException>();
        }

        [Fact]
        public void ChangingCatalogProductSku_WithValidSku_ShouldUpdateSkuProperty()
        {
            // Arrange
            var catalogProduct = new CatalogProduct("123", "product name", "Placeholder product description");
            var newSku = "456";

            // Act
            catalogProduct.SetSku(newSku);

            // Arrange
            catalogProduct.Sku.Should().Be(newSku);
        }

        [Fact]
        public void ChangingCatalogProductName_WithValidName_ShouldUpdateNameProperty()
        {
            // Arrange
            var catalogProduct = new CatalogProduct("123", "product name", "Placeholder product description");
            var newName = "new product name";

            // Act
            catalogProduct.SetName(newName);

            // Arrange
            catalogProduct.Name.Should().Be(newName);
        }

        [Fact]
        public void ChangingCatalogProductDescription_WithValidDescription_ShouldUpdateDescriptionProperty()
        {
            // Arrange
            var catalogProduct = new CatalogProduct("123", "product name", "Placeholder product description");
            var newDescription = "New product description";

            // Act
            catalogProduct.SetDescription(newDescription);

            // Arrange
            catalogProduct.Description.Should().Be(newDescription);
        }
    }
}
