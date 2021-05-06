using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
            Key = "secret"
        };
        protected TestServer BuildTestServer()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.Configure<JwtSettings>(x =>
                    {
                        //x = _jwtSettings;
                        x.ExpiryMinutes = _jwtSettings.ExpiryMinutes;
                        x.Issuer = _jwtSettings.Issuer;
                        x.Key = _jwtSettings.Key;
                    });
                });
            return new TestServer(webHostBuilder);
        }
    }
}
