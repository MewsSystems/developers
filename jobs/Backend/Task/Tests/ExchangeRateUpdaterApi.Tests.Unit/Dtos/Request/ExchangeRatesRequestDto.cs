using Newtonsoft.Json;

namespace ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request;

public class ExchangeRatesRequestDto
{
    [JsonProperty("exchangeRatesDetails")]
    public List<ExchangeRateRequestDetailsDto> ExchangeRatesToRequest { get; set; }

    public ExchangeRatesRequestDto()
    {
        ExchangeRatesToRequest = new List<ExchangeRateRequestDetailsDto>();
    }

    public void AddExchangeRate(ExchangeRateRequestDetailsDto exchangeRateRequestDetailsDto)
    {
        ExchangeRatesToRequest.Add(exchangeRateRequestDetailsDto);
    }
}