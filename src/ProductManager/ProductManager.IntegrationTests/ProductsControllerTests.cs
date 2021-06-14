using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;
using ProductManager.Web.Requests;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class ProductsControllerTests : TestServerBase, IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public ProductsControllerTests()
        {
            _server = BuildTestServer();
            _client = _server.CreateClient();
            _mapper = _server.Services.GetRequiredService<IMapper>();
        }

        public async Task RegisterAsync(string userName, string email, string password, string role = "user")
        {
            var response = await _client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Name = userName,
                Email = email,
                Password = password,
                Role = role
            });

            response.EnsureSuccessStatusCode();
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
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var response = await _client.GetAsync("Products/Browse");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CallingBrowse_WhenAuthenticatedAndNoProducts_ShouldReturnEmptyListOfProductBlockDto()
        {
            // Arrange
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var products = await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/Browse");

            // Assert
            products.Should().BeEmpty();
        }

        [Fact]
        public async Task CallingBrowse_WhenAuthenticatedAndWithProducts_ShouldReturnNonEmptyListOfProductBlockDto()
        {
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
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

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var products = await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/Browse");

            // Assert
            products.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CallingBrowse_WhenAuthenticatedAndWithProducts_ShouldReturnListOfProductBlockDtosEquivalentToExistingProducts()
        {
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
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

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);
            var productsFromService = await svc.GetAllAsync();
            var mappedProductBlocks = _mapper.Map<List<ProductBlockDto>>(productsFromService);
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var products = await _client.GetFromJsonAsync<List<ProductBlockDto>>("Products/Browse");

            // Assert
            products.Should().BeEquivalentTo(mappedProductBlocks);
        }

        [Fact]
        public async Task CallingCreate_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange/Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingCreate_WhenAuthenticatedAndNotAuthorizedAsCatalogManager_ShouldReturnForbidden()
        {
            // Arrange
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CallingCreate_WhenAuthenticatedAndAuthorizedAsCatalogManager_ShouldReturnCreated()
        {
            // Arrange
            var sku = "123";
            var productName = "product name";
            var description = "desc";

            await RegisterAsync("NewUser", "newuser@email.com", "password", "CatalogManager");
            await LoginAsync("newuser@email.com", "password", "CatalogManager");

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
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
            var sku = "123";
            var productName = "product name";
            var description = "desc";

            await RegisterAsync("NewUser", "newuser@email.com", "password", "CatalogManager");
            await LoginAsync("newuser@email.com", "password", "CatalogManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Create", new CreateProductDto
            {
                Sku = sku,
                Name = productName,
                Description = description
            });

            var retrievedProduct = await svc.GetAsync(sku);

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
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

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

            await RegisterAsync("NewUser", "newuser@email.com", "password", "CatalogManager");
            await LoginAsync("newuser@email.com", "password", "CatalogManager");

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
            var svc = _server.Services.GetRequiredService<IProductService>();
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

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password", "CatalogManager");
            await LoginAsync("newuser@email.com", "password", "CatalogManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto
            {
                Sku = sku,
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
            var svc = _server.Services.GetRequiredService<IProductService>();
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

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password", "CatalogManager");
            await LoginAsync("newuser@email.com", "password", "CatalogManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Catalog", new UpdateCatalogProductDto
            {
                Sku = sku,
                Name = newProductName,
                Description = newDescription
            });

            var retrievedProduct = await svc.GetAsync(sku);

            // Assert
            retrievedProduct.Should().NotBeNull();
            retrievedProduct.Name.Should().Be(newProductName);
            retrievedProduct.Description.Should().Be(newDescription);
        }

        [Fact]
        public async Task CallingUpdateWarehouse_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange/Act
            var response = await _client.PostAsJsonAsync("Products/Update/Warehouse", new UpdateWarehouseProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingUpdateWarehouse_WhenAuthenticatedAndNotAuthorizedAsWarehouseManager_ShouldReturnForbidden()
        {
            // Arrange
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Warehouse", new UpdateWarehouseProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CallingUpdateWarehouse_WhenAuthenticatedAndAuthorizedAsWarehouseManagerAndProductDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var sku = "123";
            var stock = 10;
            var weight = 3.4;

            await RegisterAsync("NewUser", "newuser@email.com", "password", "WarehouseManager");
            await LoginAsync("newuser@email.com", "password", "WarehouseManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Warehouse", new UpdateWarehouseProductDto
            {
                Sku = sku,
                Stock = stock,
                Weight = weight
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CallingUpdateWarehouse_WhenAuthenticatedAndAuthorizedAsWarehouseManagerAndProductExists_ShouldReturnOk()
        {
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
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
            var newStock = 10;
            var newWeight = 3.4;

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password", "WarehouseManager");
            await LoginAsync("newuser@email.com", "password", "WarehouseManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Warehouse", new UpdateWarehouseProductDto
            {
                Sku = sku,
                Stock = newStock,
                Weight = newWeight
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CallingUpdateWarehouse_WhenAuthenticatedAndAuthorizedAsWarehouseManagerAndProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
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
            var newStock = 10;
            var newWeight = 3.4;

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password", "WarehouseManager");
            await LoginAsync("newuser@email.com", "password", "WarehouseManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Warehouse", new UpdateWarehouseProductDto
            {
                Sku = sku,
                Stock = newStock,
                Weight = newWeight
            });

            var retrievedProduct = await svc.GetAsync(sku);

            // Assert
            retrievedProduct.Should().NotBeNull();
            retrievedProduct.Stock.Should().Be(newStock);
            retrievedProduct.Weight.Should().Be(newWeight);
        }

        [Fact]
        public async Task CallingUpdateSales_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange/Act
            var response = await _client.PostAsJsonAsync("Products/Update/Sales", new UpdateSalesProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallingUpdateSales_WhenAuthenticatedAndNotAuthorizedAsSalesManager_ShouldReturnForbidden()
        {
            // Arrange
            await RegisterAsync("NewUser", "newuser@email.com", "password");
            await LoginAsync("newuser@email.com", "password");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Sales", new UpdateSalesProductDto());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CallingUpdateSales_WhenAuthenticatedAndAuthorizedAsSalesManagerAndProductDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var sku = "123";
            var netPrice = 10;
            var cost = 9;
            var taxPercentage = 23;

            await RegisterAsync("NewUser", "newuser@email.com", "password", "SalesManager");
            await LoginAsync("newuser@email.com", "password", "SalesManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Sales", new UpdateSalesProductDto
            {
                Sku = sku,
                NetPrice = netPrice,
                TaxPercentage = taxPercentage,
                Cost = cost
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CallingUpdateSales_WhenAuthenticatedAndAuthorizedAsSalesManagerAndProductExists_ShouldReturnOk()
        {
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
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
            var newNetPrice = 20;
            var newCost = 14;
            var newTaxPercentage = 8;

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password", "SalesManager");
            await LoginAsync("newuser@email.com", "password", "SalesManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Sales", new UpdateSalesProductDto
            {
                Sku = sku,
                NetPrice = newNetPrice,
                TaxPercentage = newTaxPercentage,
                Cost = newCost
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CallingUpdateSales_WhenAuthenticatedAndAuthorizedAsSalesManagerAndProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var svc = _server.Services.GetRequiredService<IProductService>();
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
            var newNetPrice = 20;
            var newCost = 14;
            var newTaxPercentage = 8;

            var productDto = new ProductDto
            {
                CatalogId = catalogId,
                WarehouseId = warehouseId,
                SalesId = salesId,
                Name = productName,
                Cost = cost,
                NetPrice = netPrice,
                Description = description,
                Sku = sku,
                Stock = stock,
                TaxPercentage = taxPercentage,
                Weight = weight
            };
            await svc.AddAsync(productDto);

            await RegisterAsync("NewUser", "newuser@email.com", "password", "SalesManager");
            await LoginAsync("newuser@email.com", "password", "SalesManager");

            // Act
            var response = await _client.PostAsJsonAsync("Products/Update/Sales", new UpdateSalesProductDto
            {
                Sku = sku,
                NetPrice = newNetPrice,
                TaxPercentage = newTaxPercentage,
                Cost = newCost
            });

            var retrievedProduct = await svc.GetAsync(sku);

            // Assert
            retrievedProduct.Should().NotBeNull();
            retrievedProduct.NetPrice.Should().Be(newNetPrice);
            retrievedProduct.TaxPercentage.Should().Be(newTaxPercentage);
            retrievedProduct.Cost.Should().Be(newCost);
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
