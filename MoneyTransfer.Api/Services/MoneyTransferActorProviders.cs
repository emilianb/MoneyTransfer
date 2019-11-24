using Akka.Actor;

namespace MoneyTransfer.Api.Services
{
    public delegate IActorRef GreetingManagerActorProvider();

    public delegate IActorRef AccountManagerActorProvider();

    public delegate IActorRef TransferManagerActorProvider();
}
