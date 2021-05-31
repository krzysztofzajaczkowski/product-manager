using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddTransient<IJwtHandler, JwtHandler>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
