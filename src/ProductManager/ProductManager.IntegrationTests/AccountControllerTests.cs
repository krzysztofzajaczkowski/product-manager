using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Core.Domain;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;
using ProductManager.Web.Requests;
using Xunit;

namespace ProductManager.IntegrationTests
{
    public class AccountControllerTests : TestServerBase
    {
        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExist_ShouldReturnCreated()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();

            var response = await client.PostAsJsonAsync("account/register", new RegisterRequest
            {
                Name = "NewUser",
                Email = "newuser@email.com",
                Password = "secret"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldReturnBadRequestStatusCode()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(Guid.NewGuid(), userEmail, userName, userPassword, roleName);

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = "user@email.com",
                Password = "secret2",
                Role = roleName
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_WhenUserExistsAndValidCredentialsAndDoesNotHaveRole_ShouldReturnBadRequestStatusCode()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(Guid.NewGuid(), userEmail, userName, userPassword, roleName);

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = "user@email.com",
                Password = "secret",
                Role = "admin"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_WhenUserExistsAndValidCredentials_ShouldReturnOkStatusCode()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(Guid.NewGuid(), userEmail, userName, userPassword, roleName);

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(Guid.NewGuid(), userEmail, userName, userPassword, roleName);

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();

            var response = await client.GetAsync("account");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsync_WhenUserLoggedIn_ShouldReturnOkStatusCode()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(Guid.NewGuid(), userEmail, userName, userPassword, roleName);

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = "user"
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            response = await client.GetAsync("account");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAsync_WhenUserLoggedIn_ShouldReturnAccountDto()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userId = Guid.NewGuid();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            var accountDto = new AccountDto
            {
                Id = userId,
                Email = userEmail,
                Name = userName,
                Roles = new List<string>
                {
                    roleName
                }
            };
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(userId, userEmail, userName, userPassword, roleName);

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
            receivedAccountDto.Id.Should().Be(accountDto.Id);
            receivedAccountDto.Roles.Should().BeEquivalentTo(accountDto.Roles);
        }

        [Fact]
        public async Task GetAsyncById_WhenNotLoggedIn_ShouldReturnUnauthorizedStatusCode()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var guid = Guid.NewGuid();

            var response = await client.GetAsync($"account/{guid}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsyncById_WhenUserLoggedIn_ShouldReturnForbidden()
        {
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userId = Guid.NewGuid();
            var userName = "Username";
            var userEmail = "user@email.com";
            var userPassword = "secret";
            var roleName = "user";
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(userId, userEmail, userName, userPassword, roleName);

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
            using var server = BuildTestServer();
            using var client = server.CreateClient();
            var userId = Guid.NewGuid();
            var userName = "Admin";
            var userEmail = "admin@email.com";
            var userPassword = "secret";
            var roleName = "admin";
            var accountDto = new AccountDto
            {
                Id = userId,
                Email = userEmail,
                Name = userName,
                Roles = new List<string>
                {
                    roleName
                }
            };
            using var scope = server.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();
            await service.RegisterAsync(userId, userEmail, userName, userPassword, roleName);

            var response = await client.PostAsJsonAsync("account/login", new LoginRequest
            {
                Email = userEmail,
                Password = userPassword,
                Role = roleName
            });

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            response = await client.GetAsync($"account/{userId}");

            var receivedAccountDto = await response.Content.ReadFromJsonAsync<AccountDto>();

            receivedAccountDto.Should().BeOfType<AccountDto>();
            receivedAccountDto.Email.Should().Be(accountDto.Email);
            receivedAccountDto.Name.Should().Be(accountDto.Name);
            receivedAccountDto.Id.Should().Be(accountDto.Id);
            receivedAccountDto.Roles.Should().BeEquivalentTo(accountDto.Roles);
        }
    }
}
