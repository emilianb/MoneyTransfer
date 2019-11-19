using AutoMapper;

using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Api.Models.Mappings
{
    public class MoneyTransferMappingProfile
        : Profile
    {
        public MoneyTransferMappingProfile()
        {
            CreateMap<OpenAccount, OpenAccountCommand>()
                .ConstructUsing(
                    (OpenAccount source)
                        => OpenAccountCommand.Create(
                            source.FirstName,
                            source.LastName,
                            source.Iban,
                            source.Amount,
                            source.CurrencyCode));

            CreateMap<CloseAccount, CloseAccountCommand>()
                .ConstructUsing((CloseAccount source) => CloseAccountCommand.Create(source.Iban));

            CreateMap<TransferAmount, TransferAmountCommand>()
                .ConstructUsing(
                    (TransferAmount source)
                        => TransferAmountCommand.Create(
                            source.DebitorIban,
                            source.CreditorIban,
                            source.Amount,
                            source.CurrencyCode));
        }
    }
}
