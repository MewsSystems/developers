using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Currencies.CreateCurrency;

/// <summary>
/// Command to create a new currency using its ISO 4217 code.
/// </summary>
public record CreateCurrencyCommand(string Code) : ICommand<Result<int>>;
