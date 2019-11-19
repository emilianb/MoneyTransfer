using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MoneyTransfer.Api.Configuration
{
    public static class ExceptionsConfiguration
    {
        public static IApplicationBuilder UseExceptions(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder), $"{nameof(applicationBuilder)} should not be null.");
            }

            var environment = applicationBuilder.ApplicationServices
                .GetService<IHostingEnvironment>();

            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler(
                    builder =>
                    {
                        // Add a global exception handler middleware to the application request pipeline.
                        builder.Run(
                            async context =>
                            {
                                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                                if (exceptionHandlerFeature != null)
                                {
                                    var loggerFactory = context.RequestServices.GetService<ILoggerFactory>();

                                    if (loggerFactory != null)
                                    {
                                        var logger = loggerFactory.CreateLogger("Global exception logger");

                                        logger.LogError(
                                            500,
                                            exceptionHandlerFeature.Error,
                                            exceptionHandlerFeature.Error.Message);
                                    }
                                }

                                context.Response.StatusCode = 500;

                                await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                            });
                    });
            }

            return applicationBuilder;
        }
    }
}
