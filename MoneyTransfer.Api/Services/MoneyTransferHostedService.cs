using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyTransfer.Api.Services
{
    public class MoneyTransferHostedService
        : IHostedService
    {
        public MoneyTransferHostedService(IMoneyTransferService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), $"{nameof(service)} should not be null.");
            }

            Service = service;
        }

        public IMoneyTransferService Service { get; private set; }

        public Task StartAsync(CancellationToken cancellationToken)
            => Service.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => Service.StopAsync(cancellationToken);
    }
}
