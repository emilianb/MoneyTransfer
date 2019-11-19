using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;

using MoneyTransfer.Api.Attributes;
using MoneyTransfer.Api.Models;
using MoneyTransfer.Api.Services;
using MoneyTransfer.Service.Commands;
using System.Threading.Tasks;

namespace MoneyTransfer.Api.Controllers
{
    [Route("[controller]")]
    public class AccountsController
        : ControllerBase
    {
        public AccountsController(
            AccountService accountService,
            AccountManagerActorProvider accountManagerActorProvider,
            AutoMapper.IMapper mapper)
        {
            if (accountService == null)
            {
                throw new ArgumentNullException(nameof(accountService), $"{nameof(accountService)} should not be null.");
            }

            if (accountManagerActorProvider == null)
            {
                throw new ArgumentNullException(nameof(accountManagerActorProvider), $"{nameof(accountManagerActorProvider)} should not be null.");
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper), $"{nameof(mapper)} should not be null.");
            }

            AccountService = accountService;
            AccountManagerActorProvider = accountManagerActorProvider;
            Mapper = mapper;
        }

        private AccountService AccountService { get; }

        private AccountManagerActorProvider AccountManagerActorProvider { get; }

        private AutoMapper.IMapper Mapper { get; }

        [HttpGet("{id:length(24)}")]
        public IActionResult GetAccount(string id)
        {
            var account = AccountService.GetById(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpGet]
        public IActionResult GetAccounts()
            => Ok(AccountService.GetAll());

        [HttpPost("commands/open")]
        public async Task<IActionResult> OpenAccount([NotNull, FromBody] OpenAccount request)
        {
            var accountManagerActorRef = await AccountManagerActorProvider();

            var command = Mapper.Map<OpenAccountCommand>(request);

            accountManagerActorRef.Tell(command, ActorRefs.NoSender);

            return NoContent();
        }

        [HttpPost("commands/close")]
        public async Task<IActionResult> CloseAccount([NotNull, FromBody] CloseAccount request)
        {
            var accountManagerActorRef = await AccountManagerActorProvider();

            var command = Mapper.Map<CloseAccountCommand>(request);

            accountManagerActorRef.Tell(command, ActorRefs.NoSender);

            return NoContent();
        }
    }
}
