using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MoneyTransfer.Api.Configuration
{
    public static class MappingsConfiguration
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services), $"{nameof(services)} should not be null.");
            }

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
