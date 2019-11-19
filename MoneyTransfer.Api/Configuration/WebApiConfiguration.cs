using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MoneyTransfer.Api.Configuration
{
    public static class WebApiConfiguration
    {
        [Obsolete]
        public static IServiceCollection AddWebApi(this IServiceCollection services, Action<IMvcBuilder> configurator = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services), $"{nameof(services)} should not be null.");
            }

            var builder = services
                .AddMvcCore(
                    options =>
                    {
                        options.ReturnHttpNotAcceptable = true;

                        options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                        options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                    })
                .AddJsonOptions(options => options.UseMemberCasing())
                .AddFormatterMappings()
                .AddJsonFormatters()
                .AddCors()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (configurator != null)
            {
                var mvcBuilder = new MvcBuilder(builder.Services, builder.PartManager);
                configurator(mvcBuilder);
            }

            return services;
        }
    }
}
