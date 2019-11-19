using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

using MoneyTransfer.Api.Attributes;
using MoneyTransfer.Api.Models;
using MoneyTransfer.Api.Services;
using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Api.Controllers
{
    [Route("[controller]")]
    public class TransfersController
        : ControllerBase
    {
        public TransfersController(
            TransferManagerActorProvider transferManagerActorProvider,
            AutoMapper.IMapper mapper)
        {
            if (transferManagerActorProvider == null)
            {
                throw new ArgumentNullException(nameof(transferManagerActorProvider), $"{nameof(transferManagerActorProvider)} should not be null.");
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper), $"{nameof(mapper)} should not be null.");
            }

            TransferManagerActorProvider = transferManagerActorProvider;
            Mapper = mapper;
        }

        private TransferManagerActorProvider TransferManagerActorProvider { get; }

        private AutoMapper.IMapper Mapper { get; }

        [HttpPost("commands/transfer")]
        public async Task<IActionResult> TransferAmount([NotNull, FromBody] TransferAmount request)
        {
            var transferManagerActorRef = await TransferManagerActorProvider();

            var command = Mapper.Map<TransferAmountCommand>(request);

            transferManagerActorRef.Tell(command, ActorRefs.NoSender);

            return NoContent();
        }
    }
}
