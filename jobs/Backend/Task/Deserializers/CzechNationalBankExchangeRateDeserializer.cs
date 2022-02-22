using System;
using System.Globalization;
using ExchangeRateUpdater.DTO;
using log4net;

namespace ExchangeRateUpdater.Deserializers;

public class CzechNationalBankExchangeRateDeserializer : IExchangeRateDeserializer
{
    private const string Delimiter = "|";
    private static readonly Currency CZK = new Currency("CZK");

    private readonly ILog _log;

    public CzechNationalBankExchangeRateDeserializer()
    {
        _log = LogManager.GetLogger(GetType());
    }

    public ExchangeRate Deserialize(string serializedExchangeRate)
    {
        //Austrálie|dolar|1|AUD|15,453
        if (string.IsNullOrEmpty(serializedExchangeRate))
        {
            _log.Warn("Serialized exchange rate is null or empty. Cannot process deserialization.");
            return null;
        }
        
        var split = serializedExchangeRate.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);

        if (split.Length != 5)
        {
            _log.Warn($"Serialized exchange rate {serializedExchangeRate} does not contain expected number of parts.");
            return null;
        }


        var sourceAmount = ParseDouble(split[2]);
        if (!sourceAmount.HasValue)
        {
            _log.Warn($"Source amount {split[2]} is not valid double.");
            return null;
        }
        
        var targetAmount = ParseDouble(split[4]);
        if (!targetAmount.HasValue)
        {
            _log.Warn($"Target amount {split[4]} is not valid double.");
            return null;
        }

        if (string.IsNullOrEmpty(split[3]))
        {
            _log.Warn("Current code is null or empty.");
            return null;
        }

        var sourceCurrency = new Currency(split[3]);
        var rate = Convert.ToDecimal(targetAmount / sourceAmount);

        return new ExchangeRate(sourceCurrency, CZK, rate);
    }

    private double? ParseDouble(string input)
    {
        if (!double.TryParse(input.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return null;
        }
        return result;
    }
}