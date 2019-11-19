using Akka.Actor;
using System.Threading.Tasks;

namespace MoneyTransfer.Api.Services
{
    public delegate Task<IActorRef> GreetingManagerActorProvider();

    public delegate Task<IActorRef> AccountManagerActorProvider();

    public delegate Task<IActorRef> TransferManagerActorProvider();
}
