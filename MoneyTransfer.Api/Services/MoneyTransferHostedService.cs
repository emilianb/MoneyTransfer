using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

using MoneyTransfer.Service;

namespace MoneyTransfer.Api.Services
{
    public class MoneyTransferHostedService
        : IHostedService
    {
        public MoneyTransferHostedService(MoneyTransferService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), $"{nameof(service)} should not be null.");
            }

            Service = service;
        }

        public string ConfigurationFilePath
            => Service?.ConfigurationFilePath;

        public string EnvironmentName
            => Service?.EnvironmentName;

        public MoneyTransferService Service { get; private set; }

        public Task StartAsync(CancellationToken cancellationToken)
            => Service.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => Service.StopAsync(cancellationToken);
    }
}
