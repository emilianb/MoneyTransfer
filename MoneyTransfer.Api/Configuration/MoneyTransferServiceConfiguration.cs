using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

using MoneyTransfer.Api.Services;
using MoneyTransfer.Service;

namespace MoneyTransfer.Api.Configuration
{
    public static class MoneyTransferServiceConfiguration
    {
        public static IServiceCollection AddMoneyTransferService(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services), $"{nameof(services)} should not be null.");
            }

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var environment = serviceProvider.GetService<IHostingEnvironment>();

                var configuration = serviceProvider.GetService<IConfiguration>();
                var section = configuration.GetSection(nameof(MoneyTransferReadDatabaseSettings));

                services.Configure<MoneyTransferReadDatabaseSettings>(section);

                services.AddSingleton<IMoneyTransferReadDatabaseSettings>(provider =>
                provider.GetRequiredService<IOptions<MoneyTransferReadDatabaseSettings>>().Value);

                services.AddSingleton<AccountService>();

                services.AddSingleton(provider => {
                    var serviceConfiguration = new ServiceConfiguration
                    {
                        ConfigurationFilePath = $"{nameof(MoneyTransferService).ToLowerInvariant()}.cfg",
                        EnvironmentName = environment.EnvironmentName
                    };

                    return new MoneyTransferService(serviceConfiguration);
                });

                services.AddTransient<GreetingManagerActorProvider>(provider =>
                {
                    var moneyTransferService = provider.GetService<MoneyTransferService>();

                    return () => moneyTransferService
                        .System
                        .ActorSelection("/user/Greetings*")
                        .ResolveOne(TimeSpan.FromSeconds(10));
                });

                services.AddTransient<AccountManagerActorProvider>((provider) =>
                {
                    var moneyTransferService = provider.GetService<MoneyTransferService>();

                    return () => moneyTransferService
                        .System
                        .ActorSelection("/user/AccountManager*")
                        .ResolveOne(TimeSpan.FromSeconds(10));
                });

                services.AddTransient<TransferManagerActorProvider>(provider =>
                {
                    var moneyTransferService = provider.GetService<MoneyTransferService>();

                    return () => moneyTransferService
                        .System
                        .ActorSelection("/user/TransferManager*")
                        .ResolveOne(TimeSpan.FromSeconds(10));
                });

                services.AddHostedService<MoneyTransferHostedService>();
            }

            return services;
        }
    }
}
