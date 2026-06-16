using Domain.Entities;

namespace Domain.Ports;

public interface IExchangeRatesRepository
{
    Task<List<ExchangeRate>> GetDailyExchangeRatesAsync(DateTime date, CancellationToken cancellationToken);
}