using ApplicationLayer.DTOs.Currencies;
using SOAP.Models.Currencies;

namespace SOAP.Converters;

/// <summary>
/// Converter extensions for currency DTOs to SOAP models.
/// </summary>
public static class CurrencySoapConverters
{
    /// <summary>
    /// Converts a CurrencyDto to SOAP model.
    /// </summary>
    public static CurrencySoap ToSoap(this CurrencyDto dto)
    {
        return new CurrencySoap
        {
            Id = dto.Id,
            Code = dto.Code
        };
    }

    /// <summary>
    /// Converts a collection of CurrencyDto to SOAP models.
    /// </summary>
    public static CurrencySoap[] ToSoap(this IEnumerable<CurrencyDto> dtos)
    {
        return dtos.Select(dto => dto.ToSoap()).ToArray();
    }
}
