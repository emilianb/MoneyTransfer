using Akka.Actor;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyTransfer
{
    public interface IMoneyTransferService
    {
        string ConfigurationFilePath { get; }

        string EnvironmentName { get; }

        IActorRef GreetingManager { get; }

        IActorRef AccountManager { get; }

        IActorRef TransferManager { get; }

        IActorRef MaterializeAccountsView { get; }

        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
