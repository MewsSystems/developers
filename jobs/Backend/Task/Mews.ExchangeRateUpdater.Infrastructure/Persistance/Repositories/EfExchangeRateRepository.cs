using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Mews.ExchangeRateUpdater.Infrastructure.Persistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateUpdater.Infrastructure.Persistance.Repositories;

public class EfExchangeRateRepository : IExchangeRateRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<EfExchangeRateRepository> _logger;

    public EfExchangeRateRepository(AppDbContext db, ILogger<EfExchangeRateRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    
    public async Task UpsertRatesAsync(IEnumerable<ExchangeRate> rates, DateTime date, CancellationToken ct)
    {
        _logger.LogInformation("Upserting {Count} exchange rates for {Date}", rates.Count(), date);

        // Get all existing entities regardless of date
        var allEntities = await _db.ExchangeRates.ToListAsync(ct);

        foreach (var rate in rates)
        {
            var match = allEntities.FirstOrDefault(e =>
                e.SourceCurrency == rate.SourceCurrency.Code &&
                e.TargetCurrency == rate.TargetCurrency.Code);

            if (match is null)
            {
                _logger.LogDebug("Inserting rate {Source}->{Target}={Value} for {Date}",
                    rate.SourceCurrency, rate.TargetCurrency, rate.Value, date);

                _db.ExchangeRates.Add(new ExchangeRateEntity
                {
                    Date = date,
                    SourceCurrency = rate.SourceCurrency.Code,
                    TargetCurrency = rate.TargetCurrency.Code,
                    Value = rate.Value
                });
            }
            else
            {
                _logger.LogDebug("Updating rate {Source}->{Target}={Value} for {Date}",
                    rate.SourceCurrency, rate.TargetCurrency, rate.Value, date);

                match.Value = rate.Value;
                match.Date = date; // Se actualiza la fecha
            }
        }

        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("Exchange rates saved to DB.");
    }

    public async Task<IEnumerable<ExchangeRate>> GetRatesAsync(DateTime date, IEnumerable<Currency> currencies, CancellationToken ct)
    {
        var codes = currencies.Select(c => c.Code).ToHashSet();
        
        _logger.LogInformation("Fetching rates for {Count} currencies for {Date}", codes.Count, date);

        var entities = await _db.ExchangeRates
            .Where(r => codes.Contains(r.SourceCurrency))
            .ToListAsync(ct);
        
        _logger.LogDebug("Fetched {Count} rates from DB", entities.Count);

        return entities.Select(e => new ExchangeRate(new Currency(e.SourceCurrency), new Currency(e.TargetCurrency), e.Value));
    }

    public async Task<bool> HasRatesForDateAsync(DateTime date, CancellationToken ct)
    {
        var exists = await _db.ExchangeRates.AnyAsync(r => r.Date == date, ct);
        _logger.LogDebug("Rates for {Date} exist: {Exists}", date, exists);
        return exists;
    }
}