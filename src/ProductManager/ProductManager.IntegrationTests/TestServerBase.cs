using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Repositories;
using ProductManager.Infrastructure.Settings;
using ProductManager.Web;

namespace ProductManager.IntegrationTests
{
    public class TestServerBase
    {
        protected JwtSettings _jwtSettings = new JwtSettings
        {
            ExpiryMinutes = 1,
            Issuer = "testIssuer",
            Key = "secretKeyDED15CCD-AACA-4CCE-8AA4-180938F052C9"
        };
        protected TestServer BuildTestServer()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(IUserRepository));
                    services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                    services.Configure<JwtSettings>(x =>
                    {
                        x.ExpiryMinutes = _jwtSettings.ExpiryMinutes;
                        x.Issuer = _jwtSettings.Issuer;
                        x.Key = _jwtSettings.Key;
                    });
                });
            return new TestServer(webHostBuilder);
        }
    }
}
