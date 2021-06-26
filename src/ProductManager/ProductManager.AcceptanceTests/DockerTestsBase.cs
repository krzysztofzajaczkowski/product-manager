using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using ProductManager.Infrastructure.DTO;
using ProductManager.Web.Requests;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ProductManager.AcceptanceTests
{
    public class DockerTestsBase : IDisposable
    {
        private readonly string _dirSeparator;
        protected ITestOutputHelper TestOutputHelper { get; }
        protected ICompositeService CompositeService { get; }

        public DockerTestsBase(ITestOutputHelper testOutputHelper)
        {
            Environment.SetEnvironmentVariable("RUN_ELECTRON", "0");

            TestOutputHelper = testOutputHelper;
            _dirSeparator = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _dirSeparator = "/";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _dirSeparator = @"\";
            }
            var testAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var solutionPath = Directory.GetParent(testAssemblyPath.Substring(0, testAssemblyPath.LastIndexOf($@"{_dirSeparator}bin{_dirSeparator}", StringComparison.Ordinal))).FullName;
            var projectPath = $"{solutionPath}{_dirSeparator}ProductManager.Web";

            var composePath = $"{solutionPath}{_dirSeparator}acceptance-tests-docker-compose.yml";
            CompositeService = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(composePath)
                .RemoveOrphans()
                .WaitForHttp("app", "http://localhost:8006", continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0)
                .Build().Start();
        }

        protected Task<HttpClient> SetupConnection(string hostUrl)
        {
            return Task.FromResult(new HttpClient
            {
                BaseAddress = new Uri($"http://{hostUrl}"),
            });
        }

        protected async Task<JwtDto> LoginClientAsync(HttpClient client, string email, string password, string role)
        {
            var response = await client.PostAsJsonAsync("api/account", new LoginRequest
            {
                Email = email,
                Password = password,
                Role = role
            });
            return await response.Content.ReadFromJsonAsync<JwtDto>();
        }

        public void Dispose()
        {
            CompositeService.Remove();
            CompositeService.Dispose();
        }
    }
}
