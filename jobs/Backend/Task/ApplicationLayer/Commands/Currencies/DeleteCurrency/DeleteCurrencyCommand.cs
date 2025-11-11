using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Currencies.DeleteCurrency;

/// <summary>
/// Command to delete a currency.
/// Only allows deletion if the currency is not being used by any providers or exchange rates.
/// </summary>
public record DeleteCurrencyCommand(int CurrencyId, bool Force = false) : ICommand<Result>;
