using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.Repositories;
using ProductManager.Infrastructure.Services;
using ProductManager.Infrastructure.Settings;

namespace ProductManager.Infrastructure.IoC
{
    public static class InfrastructureContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection("jwt"));

            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddTransient<IJwtHandler, JwtHandler>();

            return services;
        }
    }
}
