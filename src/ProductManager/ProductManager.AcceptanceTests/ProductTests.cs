using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;
using ProductManager.Web.Requests;
using Xunit;
using Xunit.Abstractions;

namespace ProductManager.AcceptanceTests
{
    [Collection("DockerTests")]
    public class ProductTests : DockerTestsBase, IDisposable
    {
        private readonly HttpClientFixture _httpClientFixture;
        private readonly HttpClient _client;

        public ProductTests(ITestOutputHelper testOutputHelper, HttpClientFixture httpClientFixture) : base(testOutputHelper)
        {
            _httpClientFixture = httpClientFixture;
            _client = httpClientFixture.Client;
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:8006");
            }
        }

        public async Task LoginAsync(string email, string password, string role = "user")
        {
            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = email,
                Password = password,
                Role = role
            });

            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenDto.Token}");
        }

        [Fact]
        public async Task CallingBrowse_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange/Act
            var response = await _client.GetAsync("Products/Browse");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingBrowse_WhenAuthenticated_ShouldReturnOk()
        {
            // Arrange
            await LoginAsync("admin@admin.com", "secret");

            // Act
            var response = await _client.GetAsync("Products/Browse");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CallingBrowse_WhenAuthenticatedAndWithProducts_ShouldReturnNonEmptyListOfProductBlockDto()
        {
            //Arrange
            await LoginAsync("admin@admin.com", "secret");

            // Act
            var products = await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/Browse");

            // Assert
            products.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CallingGet_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            //Arrange/Act
            var sku = "123";
            var response = await _client.GetAsync($"Products/Browse/{sku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingGet_WhenAuthenticatedAndProductExists_ShouldReturnOk()
        {
            //Arrange
            await LoginAsync("admin@admin.com", "secret");
            var someProductSku = (await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/browse")).First().Sku;

            // Act
            var response = await _client.GetAsync($"Products/Browse/{someProductSku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CallingGet_WhenAuthenticatedAndProductDoesNotExist_ShouldReturnNotFound()
        {
            //Arrange
            var sku = "123";
            await LoginAsync("admin@admin.com", "secret");

            // Act
            var response = await _client.GetAsync($"Products/Browse/{sku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CallingGet_WhenAuthenticatedAndProductExists_ShouldReturnProductDto()
        {
            //Arrange
            await LoginAsync("admin@admin.com", "secret");
            var someProductSku = (await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/browse")).First().Sku;

            // Act
            var product = await _client.GetFromJsonAsync<ProductDto>($"Products/Browse/{someProductSku}");

            // Assert
            product.Should().BeOfType<ProductDto>();
        }

        [Fact]
        public async Task CallingCreate_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            //Arrange/Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingCreate_WhenAuthenticatedAndNotAuthorizedAsCatalogManager_ShouldReturnForbidden()
        {
            //Arrange
            await LoginAsync("admin@admin.com", "secret");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CallingCreate_WhenAuthenticatedAndAuthorizedAsCatalogManager_ShouldReturnCreated()
        {
            //Arrange
            var sku = "123";
            var productName = "product name";
            var description = "desc";

            await LoginAsync("admin@admin.com", "secret", "CatalogManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto
            {
                Sku = sku,
                Name = productName,
                Description = description
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CallingCreate_WhenAuthenticatedAndAuthorizedAsCatalogManager_ShouldCreateNewProduct()
        {
            //Arrange
            var sku = "123";
            var productName = "product name";
            var description = "desc";

            await LoginAsync("admin@admin.com", "secret", "CatalogManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto
            {
                Sku = sku,
                Name = productName,
                Description = description
            });

            response.EnsureSuccessStatusCode();

            var retrievedProduct = await _client.GetFromJsonAsync<ProductDto>($"Products/browse/{sku}");

            // Assert
            retrievedProduct.Should().NotBeNull();
        }

        [Fact]
        public async Task CallingUpdateCatalog_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange/Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingUpdateCatalog_WhenAuthenticatedAndNotAuthorizedAsCatalogManager_ShouldReturnForbidden()
        {
            // Arrange
            await LoginAsync("admin@admin.com", "secret");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CallingUpdateCatalog_WhenAuthenticatedAndAuthorizedAsCatalogManagerAndProductDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var sku = "123";
            var productName = "product name";
            var description = "desc";

            await LoginAsync("admin@admin.com", "secret", "CatalogManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto
            {
                Sku = sku,
                Name = productName,
                Description = description
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CallingUpdateCatalog_WhenAuthenticatedAndAuthorizedAsCatalogManagerAndProductExists_ShouldReturnOk()
        {
            // Arrange
            var newProductName = "product name 2";
            var newDescription = "desc 2";

            await LoginAsync("admin@admin.com", "secret", "CatalogManager");
            var someProductSku = (await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/browse")).First().Sku;

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto
            {
                Sku = someProductSku,
                Name = newProductName,
                Description = newDescription
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CallingUpdateCatalog_WhenAuthenticatedAndAuthorizedAsCatalogManagerAndProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var newProductName = "product name 2";
            var newDescription = "desc 2";

            await LoginAsync("admin@admin.com", "secret", "CatalogManager");
            var someProductSku = (await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/browse")).First().Sku;

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto
            {
                Sku = someProductSku,
                Name = newProductName,
                Description = newDescription
            });

            var retrievedProduct = await _client.GetFromJsonAsync<ProductDto>($"Products/browse/{someProductSku}");

            // Assert
            retrievedProduct.Should().NotBeNull();
            retrievedProduct.Name.Should().Be(newProductName);
            retrievedProduct.Description.Should().Be(newDescription);
        }

        public new void Dispose()
        {
            base.Dispose();
            _client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
