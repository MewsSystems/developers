
namespace ExchangeRateUpdater.Models.Requests;
public class ExchangeRateRequestDto
{
    public DateTime Date { get; set; }
    public List<ExchangeRateRequest> ExchangeRatesDetails { get; set; }
}
