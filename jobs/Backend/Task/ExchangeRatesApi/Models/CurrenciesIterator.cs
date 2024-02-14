using ExchangeRatesService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatesApi.Models;

public class CurrenciesIterator
{
    [FromQuery(Name = "codes")]
    public List<Currency> Codes { get; set; }
}