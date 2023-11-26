using Newtonsoft.Json;

namespace ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request;

public class CurrencyDto
{
    [JsonProperty("code")]
    public string Code { get; set; }

    public CurrencyDto(string code)
    {
        Code = code;
    }
}