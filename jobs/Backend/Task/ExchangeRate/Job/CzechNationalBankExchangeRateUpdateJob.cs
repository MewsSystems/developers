using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Service;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Job
{
    public class CzechNationalBankExchangeRateUpdateJob(IExchangeRateService exchangeRateService, ILogger<CzechNationalBankExchangeRateUpdateJob> logger) : IJob
    {
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly ILogger<CzechNationalBankExchangeRateUpdateJob> _logger = logger;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"{nameof(CzechNationalBankExchangeRateUpdateJob)} - Updating exchange rates.");
                await _exchangeRateService.UpdateDailyExchangeRatesAsync(new Currency("CZK"), DateOnly.FromDateTime(DateTime.Today), new CancellationToken());
                _logger.LogInformation($"{nameof(CzechNationalBankExchangeRateUpdateJob)} - Exchange rates updated successfully.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CzechNationalBankExchangeRateUpdateJob)} An error occurred while updating exchange rates.");
            }
        }
    }
}
