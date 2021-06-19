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
using ProductManager.Infrastructure.Database;
using ProductManager.Infrastructure.Repositories;
using ProductManager.Infrastructure.Settings;
using ProductManager.Web;

namespace ProductManager.IntegrationTests
{
    public class TestServerBase : IDisposable
    {
        protected JwtSettings _jwtSettings;
        private string _dbFileName;

        protected TestServer BuildTestServer(bool inMemoryRepositories = false)
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
                    if (inMemoryRepositories)
                    {
                        services.RemoveAll(typeof(IUserRepository));
                        services.AddSingleton<IUserRepository, InMemoryUserRepository>();

                        services.RemoveAll(typeof(IProductRepository));
                        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
                    }
                    else
                    {
                        _dbFileName = $"database{Guid.NewGuid()}.db";
                        services.RemoveAll(typeof(IDbConnectionFactory));
                        services.AddTransient<IDbConnectionFactory, SQLiteConnectionFactory>(serviceProvider =>
                            new SQLiteConnectionFactory($"DataSource={_dbFileName};BinaryGUID=False;"));
                        //var provider = services.BuildServiceProvider();
                        //var dbFileName = provider.GetRequiredService<IConfiguration>().GetConnectionString("SQLite").Split(";").First().Split("=").Last();
                        //if (File.Exists(_dbFileName))
                        //{
                        //    File.Delete(_dbFileName);
                        //}
                    }
                });

            var server = new TestServer(webHostBuilder);

            _jwtSettings = server.Services.GetService<IOptions<JwtSettings>>().Value;

            return server;
        }

        public void Dispose()
        {
            if (!string.IsNullOrWhiteSpace(_dbFileName) && File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
        }
    }
}
