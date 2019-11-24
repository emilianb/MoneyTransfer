using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;

using MoneyTransfer.Api.Attributes;
using MoneyTransfer.Api.Models;
using MoneyTransfer.Api.Services;
using MoneyTransfer.Messages;

namespace MoneyTransfer.Api.Controllers
{
    [Route("[controller]")]
    public class AccountsController
        : ControllerBase
    {
        public AccountsController(
            IAccountService accountService,
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

        private IAccountService AccountService { get; }

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
        public IActionResult OpenAccount([NotNull, FromBody] OpenAccount request)
        {
            var accountManagerActorRef = AccountManagerActorProvider();

            var command = Mapper.Map<AccountCommands.Open>(request);

            accountManagerActorRef.Tell(command, ActorRefs.NoSender);

            return NoContent();
        }

        [HttpPost("commands/close")]
        public IActionResult CloseAccount([NotNull, FromBody] CloseAccount request)
        {
            var accountManagerActorRef = AccountManagerActorProvider();

            var command = Mapper.Map<AccountCommands.Close>(request);

            accountManagerActorRef.Tell(command, ActorRefs.NoSender);

            return NoContent();
        }
    }
}
