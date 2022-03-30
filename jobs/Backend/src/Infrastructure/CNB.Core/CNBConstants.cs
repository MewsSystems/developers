using System.Globalization;

namespace ExchangeRate.Infrastructure.CNB.Core;

public static class CNBConstants
{
    public const string DateFormat = "dd.MM.yyyy";

    public static readonly NumberFormatInfo RateFormat = new() { NumberDecimalSeparator = "," };
}
