using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

using MoneyTransfer.Api.Services;
using MoneyTransfer.Configuration.Mappings;

namespace MoneyTransfer.Api.Configuration
{
    public static class MoneyTransferConfiguration
    {
        public static IServiceCollection AddMoneyTransfer(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services), $"{nameof(services)} should not be null.");
            }

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var environment = serviceProvider.GetService<IHostingEnvironment>();

                var configuration = serviceProvider.GetService<IConfiguration>();
                var section = configuration.GetSection("MoneyTransferDatabase");

                services.Configure<DatabaseSettings>(section);

                services.AddSingleton(provider => provider.GetRequiredService<IOptions<DatabaseSettings>>().Value);

                services.AddTransient<IAccountService>(provider =>
                {
                    var databaseSettings = provider.GetService<DatabaseSettings>();

                    return new AccountService(databaseSettings.ConnectionString);
                });

                services.AddSingleton<IMoneyTransferService>(provider =>
                {
                    var serviceConfiguration = new ServiceConfiguration
                    {
                        ConfigurationFilePath = $"{nameof(MoneyTransferService).ToLowerInvariant()}.conf",
                        EnvironmentName = environment.EnvironmentName
                    };

                    return new MoneyTransferService(serviceConfiguration);
                });

                services.AddTransient<GreetingManagerActorProvider>(provider =>
                {
                    var moneyTransferService = provider.GetService<IMoneyTransferService>();

                    return () => moneyTransferService.GreetingManager;
                });

                services.AddTransient<AccountManagerActorProvider>((provider) =>
                {
                    var moneyTransferService = provider.GetService<IMoneyTransferService>();

                    return () => moneyTransferService.AccountManager;
                });

                services.AddTransient<TransferManagerActorProvider>(provider =>
                {
                    var moneyTransferService = provider.GetService<IMoneyTransferService>();

                    return () => moneyTransferService.TransferManager;
                });

                services.AddHostedService<MoneyTransferHostedService>();
            }

            var mappingTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => typeof(IHasBsonMappings).IsAssignableFrom(t) && typeof(IHasBsonMappings) != t))
                .ToList();

            foreach (var mappingType in mappingTypes)
            {
                var mapper = (IHasBsonMappings)Activator.CreateInstance(mappingType);
                mapper.Register();
            }

            return services;
        }
    }
}
