using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.MongoDb;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MoneyTransfer.Actors;

namespace MoneyTransfer
{
    public class MoneyTransferService
        : IMoneyTransferService
    {
        public MoneyTransferService(ServiceConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} should not be null.");
            }

            ConfigurationFilePath = configuration.ConfigurationFilePath;
            EnvironmentName = configuration.EnvironmentName;
        }

        private ActorSystem System { get; set; }

        public string ConfigurationFilePath { get; private set; }

        public string EnvironmentName { get; private set; }

        public IActorRef GreetingManager { get; private set; }

        public IActorRef AccountManager { get; private set; }

        public IActorRef TransferManager { get; private set; }

        public IActorRef MaterializeAccountsView { get; private set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var configurationString = File.ReadAllText(ConfigurationFilePath);

            var config = ConfigurationFactory.ParseString(configurationString);

            var environmentConfigFilePath = Path.Combine(
                Path.GetDirectoryName(ConfigurationFilePath),
                $"{Path.GetFileNameWithoutExtension(ConfigurationFilePath)}.{EnvironmentName}{Path.GetExtension(ConfigurationFilePath)}");

            if (File.Exists(environmentConfigFilePath))
            {
                configurationString = File.ReadAllText(environmentConfigFilePath);

                config = ConfigurationFactory.ParseString(configurationString).WithFallback(config);
            }

            System = ActorSystem.Create("MoneyTransferSystem", config);

            MongoDbPersistence.Get(System);

            GreetingManager = System.ActorOf(Props.Create(() => new GreetingManagerActor()), "GreetingManager");

            AccountManager = System.ActorOf(Props.Create(() => new AccountManagerActor()), "AccountManager");

            TransferManager = System.ActorOf(Props.Create(() => new TransferManagerActor()), "TransferManager");

            MaterializeAccountsView = System.ActorOf(Props.Create(() => new MaterializeAccountsViewActor()), "MaterializeAccountsView");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => CoordinatedShutdown.Get(System).Run(CoordinatedShutdown.ClrExitReason.Instance);
    }
}
