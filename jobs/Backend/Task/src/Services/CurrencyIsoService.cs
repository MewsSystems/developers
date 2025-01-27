using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
    public interface ICurrencyIsoService
    {
        HashSet<string> GetValidIsoCodes();
        void ValidateCode(string code);
    }

    public class CurrencyIsoService : ICurrencyIsoService
    {
        private readonly ILogger<CurrencyIsoService> _logger;
        private readonly HashSet<string> _validIsoCodes;
        private readonly string _isoCodesFilePath;

        public CurrencyIsoService(
            IOptions<CurrencyOptions> options,
            ILogger<CurrencyIsoService> logger)
        {
            if (options?.Value?.IsoCodesFilePath == null)
                throw new ArgumentNullException(nameof(options), "Currency configuration is missing IsoCodesFilePath");

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isoCodesFilePath = options.Value.IsoCodesFilePath;
            _validIsoCodes = LoadIsoCodes();
        }

        public HashSet<string> GetValidIsoCodes()
        {
            return _validIsoCodes;
        }
        
        
        public void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be empty or whitespace.", nameof(code));
            }

            if (code.Length != 3)
            {
                throw new ArgumentException("Currency code must be exactly 3 characters long.", nameof(code));
            }

            if (!code.All(char.IsLetter))
            {
                throw new ArgumentException("Currency code must contain only letters.", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();
            if (!_validIsoCodes.Contains(normalizedCode))
            {
                throw new ArgumentException($"'{normalizedCode}' is not a valid ISO 4217 currency code.", nameof(code));
            }
        }

        private HashSet<string> LoadIsoCodes()
        {
            try
            {
                _logger.LogInformation("Loading ISO currency codes from {FilePath}", _isoCodesFilePath);
                
                if (!File.Exists(_isoCodesFilePath))
                {
                    throw new FileNotFoundException($"Currency codes file not found at {_isoCodesFilePath}");
                }

                var jsonContent = File.ReadAllText(_isoCodesFilePath);
                var currencyData = JsonSerializer.Deserialize<List<CurrencyIsoData>>(jsonContent);

                var codes = new HashSet<string>(
                    currencyData
                        .Where(x => x.IsActive && !string.IsNullOrEmpty(x.AlphabeticCode))
                        .Select(x => x.AlphabeticCode)
                );

                _logger.LogInformation("Loaded {Count} active ISO currency codes", codes.Count);
                return codes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading ISO currency codes from file {FilePath}", _isoCodesFilePath);
                throw new InvalidOperationException($"Failed to load currency codes from {_isoCodesFilePath}", ex);
            }
        }
    }
} 