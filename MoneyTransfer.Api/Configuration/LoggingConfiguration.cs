using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace MoneyTransfer.Api.Configuration
{
    public static class LoggingConfiguration
    {
        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder webHost)
        {
            if (webHost == null)
            {
                throw new ArgumentNullException(nameof(webHost), $"{nameof(webHost)} should not be null.");
            }

            webHost
                .ConfigureLogging((ILoggingBuilder configuration) => configuration.ClearProviders())
                .UseSerilog((webHostContext, configuration) => configuration.ReadFrom.Configuration(webHostContext.Configuration));

            return webHost;
        }
    }
}
