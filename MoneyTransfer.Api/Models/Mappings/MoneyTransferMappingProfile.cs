using AutoMapper;

using MoneyTransfer.Messages;

namespace MoneyTransfer.Api.Models.Mappings
{
    public class MoneyTransferMappingProfile
        : Profile
    {
        public MoneyTransferMappingProfile()
        {
            CreateMap<OpenAccount, AccountCommands.Open>()
                .ConstructUsing(
                    (OpenAccount source)
                        => new AccountCommands.Open(
                            source.FirstName,
                            source.LastName,
                            source.Iban,
                            source.Amount,
                            source.CurrencyCode));

            CreateMap<CloseAccount, AccountCommands.Close>()
                .ConstructUsing((CloseAccount source) => new AccountCommands.Close(source.Iban));

            CreateMap<Transfer, TransferCommand>()
                .ConstructUsing(
                    (Transfer source)
                        => TransferCommand.Create(
                            source.DebitorIban,
                            source.CreditorIban,
                            source.Amount,
                            source.CurrencyCode));
        }
    }
}
