using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Repositories;
using ProductManager.Infrastructure.Settings;
using ProductManager.Web;

namespace ProductManager.IntegrationTests
{
    public class TestServerBase
    {
        protected JwtSettings _jwtSettings;

        protected TestServer BuildTestServer()
        {
            var testAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var solutionPath = Directory.GetParent(testAssemblyPath.Substring(0, testAssemblyPath.LastIndexOf($@"\bin\", StringComparison.Ordinal))).FullName;
            var appsettingsPath = Path.Join(solutionPath, "ProductManager.Web");

            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(appsettingsPath)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(IUserRepository));
                    services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                });

            var server = new TestServer(webHostBuilder);

            _jwtSettings = server.Services.GetService<IOptions<JwtSettings>>().Value;

            return server;
        }
    }
}
