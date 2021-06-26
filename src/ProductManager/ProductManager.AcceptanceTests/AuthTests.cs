using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class AuthTests : DockerTestsBase, IDisposable
    {
        private readonly HttpClientFixture _httpClientFixture;
        private readonly HttpClient _client;

        public AuthTests(ITestOutputHelper testOutputHelper, HttpClientFixture httpClientFixture) : base(testOutputHelper)
        {
            _httpClientFixture = httpClientFixture;
            _client = httpClientFixture.Client;
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:8006");
            }
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExist_ShouldReturnCreated()
        {
            
            var response = await _client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Name = "NewUser",
                Email = "newuser@email.com",
                Password = "secret",
                Role = "user"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExistAndNameNotSpecified_ShouldReturnBadRequest()
        {
            
            var response = await _client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Email = "newuser@email.com",
                Password = "secret",
                Role = "user"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExistAndEmailNotSpecified_ShouldReturnBadRequest()
        {
            
            var response = await _client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Name = "NewUser",
                Password = "secret",
                Role = "user"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExistAndPasswordNotSpecified_ShouldReturnBadRequest()
        {
            
            var response = await _client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Name = "NewUser",
                Email = "newuser@email.com",
                Role = "user"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExistAndRoleNotSpecified_ShouldReturnBadRequest()
        {
            
            var response = await _client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Name = "NewUser",
                Email = "newuser@email.com",
                Password = "secret"
            });

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldReturnBadRequestStatusCode()
        {
            
            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = "user@email.com",
                Password = "secret",
                Role = "user"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_WhenUserExistsAndInvalidCredentials_ShouldReturnBadRequestStatusCode()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = $"{userPassword}1",
                Role = roleName
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_WhenUserExistsAndValidCredentialsAndDoesNotHaveRole_ShouldReturnBadRequestStatusCode()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "nonExistentRole";

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_WhenUserExistsAndValidCredentials_ShouldReturnOkStatusCode()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task LoginAsync_WhenUserExistsAndValidCredentials_ShouldReturnTokenDto()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            tokenDto.Should().BeOfType<TokenDto>();
        }

        [Fact]
        public async Task GetAsync_WhenNotLoggedIn_ShouldReturnUnauthorizedStatusCode()
        {
            
            var response = await _client.GetAsync("account");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsync_WhenUserLoggedIn_ShouldReturnOkStatusCode()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            response = await _client.GetAsync("account");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAsync_WhenUserLoggedIn_ShouldReturnAccountDto()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";
            var accountDto = new AccountDto
            {
                Email = userEmail,
                Name = userName,
                Roles = new List<string>
                {
                    "SalesManager",
                    "user",
                    "WarehouseManager",
                    "admin",
                    "CatalogManager"
                }
            };


            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });
            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            var receivedAccountDto = await _client.GetFromJsonAsync<AccountDto>("account");

            receivedAccountDto.Should().BeOfType<AccountDto>();
            receivedAccountDto.Email.Should().Be(accountDto.Email);
            receivedAccountDto.Name.Should().Be(accountDto.Name);
            receivedAccountDto.Roles.Should().BeEquivalentTo(accountDto.Roles);
        }

        [Fact]
        public async Task GetAsyncById_WhenNotLoggedIn_ShouldReturnUnauthorizedStatusCode()
        {
                        var guid = Guid.NewGuid();

            var response = await _client.GetAsync($"account/{guid}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsyncById_WhenUserLoggedIn_ShouldReturnForbidden()
        {
                        var userId = Guid.NewGuid();
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "user";

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            response = await _client.GetAsync($"account/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        }

        [Fact]
        public async Task GetAsyncById_WhenAdminLoggedIn_ShouldReturnAccountDto()
        {
                        var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "admin";
            var accountDto = new AccountDto
            {
                Email = userEmail,
                Name = userName,
                Roles = new List<string>
                {
                    "SalesManager",
                    "user",
                    "WarehouseManager",
                    "admin",
                    "CatalogManager"
                }
            };

            var response = await _client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            var receivedAccountDto = await _client.GetFromJsonAsync<AccountDto>("account");

            response = await _client.GetAsync($"account/{receivedAccountDto.Id}");

            receivedAccountDto = await response.Content.ReadFromJsonAsync<AccountDto>();

            receivedAccountDto.Should().BeOfType<AccountDto>();
            receivedAccountDto.Email.Should().Be(accountDto.Email);
            receivedAccountDto.Name.Should().Be(accountDto.Name);
            receivedAccountDto.Roles.Should().BeEquivalentTo(accountDto.Roles);
        }

        public new void Dispose()
        {
            base.Dispose();
            _client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
