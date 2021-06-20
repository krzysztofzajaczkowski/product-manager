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
    public class AuthTests : DockerTestsBase
    {
        private readonly Task _clientsBootstrapTask;
        private HttpClient _client;

        public AuthTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _clientsBootstrapTask = Task.Run(async () =>
            {
                _client = await SetupConnection("localhost:8006");
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExist_ShouldReturnCreated()
        {
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.PostAsJsonAsync("account/register", new RegisterRequest
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
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.PostAsJsonAsync("account/register", new RegisterRequest
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
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.PostAsJsonAsync("account/register", new RegisterRequest
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
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.PostAsJsonAsync("account/register", new RegisterRequest
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
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.PostAsJsonAsync("account/register", new RegisterRequest
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
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
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
            await _clientsBootstrapTask;
            using var client = _client;
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
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
            await _clientsBootstrapTask;
            using var client = _client;
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "nonExistentRole";

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
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
            await _clientsBootstrapTask;
            using var client = _client;
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
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
            await _clientsBootstrapTask;
            using var client = _client;
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
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
            await _clientsBootstrapTask;
            using var client = _client;

            var response = await client.GetAsync("account");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsync_WhenUserLoggedIn_ShouldReturnOkStatusCode()
        {
            await _clientsBootstrapTask;
            using var client = _client;
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "CatalogManager";

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            response = await client.GetAsync("account");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAsync_WhenUserLoggedIn_ShouldReturnAccountDto()
        {
            await _clientsBootstrapTask;
            using var client = _client;
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


            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });
            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            var receivedAccountDto = await client.GetFromJsonAsync<AccountDto>("account");

            receivedAccountDto.Should().BeOfType<AccountDto>();
            receivedAccountDto.Email.Should().Be(accountDto.Email);
            receivedAccountDto.Name.Should().Be(accountDto.Name);
            receivedAccountDto.Roles.Should().BeEquivalentTo(accountDto.Roles);
        }

        [Fact]
        public async Task GetAsyncById_WhenNotLoggedIn_ShouldReturnUnauthorizedStatusCode()
        {
            await _clientsBootstrapTask;
            using var client = _client;
            var guid = Guid.NewGuid();

            var response = await client.GetAsync($"account/{guid}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsyncById_WhenUserLoggedIn_ShouldReturnForbidden()
        {
            await _clientsBootstrapTask;
            using var client = _client;
            var userId = Guid.NewGuid();
            var userName = "admin";
            var userEmail = "admin@admin.com";
            var userPassword = "secret";
            var roleName = "user";

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            response = await client.GetAsync($"account/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        }

        [Fact]
        public async Task GetAsyncById_WhenAdminLoggedIn_ShouldReturnAccountDto()
        {
            await _clientsBootstrapTask;
            using var client = _client;
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

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            var receivedAccountDto = await client.GetFromJsonAsync<AccountDto>("account");

            response = await client.GetAsync($"account/{receivedAccountDto.Id}");

            receivedAccountDto = await response.Content.ReadFromJsonAsync<AccountDto>();

            receivedAccountDto.Should().BeOfType<AccountDto>();
            receivedAccountDto.Email.Should().Be(accountDto.Email);
            receivedAccountDto.Name.Should().Be(accountDto.Name);
            receivedAccountDto.Roles.Should().BeEquivalentTo(accountDto.Roles);
        }
    }
}
