using ExchangeRatesUpdater.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;

namespace ExchangeRatesUpdater;

internal class ParametersParser
{
    private readonly ILogger logger;
    private readonly AppConfiguration config;

    private readonly string[] defaultCurrencies;
    private readonly string defaultBank;

    public ParametersParser(IOptions<AppConfiguration> config, ILogger<ParametersParser> logger)
    {
        this.config = config.Value;
        this.logger = logger;

        defaultCurrencies = this.config.DefaultCurrencies;
        defaultBank = this.config.DefaultBank;

        if (defaultBank == string.Empty || defaultCurrencies.Length == 0)
            logger.LogError("Default bank or currencies not set in appsettings.json");
    }

    internal IDictionary<string, IEnumerable<string>> Parse(string[] args)
    {
        if (args.Length == 0) {
            logger.LogInformation("No arguments supplied, using defaults.");
            return new Dictionary<string, IEnumerable<string>> { { defaultBank, defaultCurrencies } }; ;
        }

        if (args.Length % 2 != 0) {
            logger.LogError("Invalid number of arguments supplied ({amount}), using defaults.", args.Length);
            return new Dictionary<string, IEnumerable<string>> { { defaultBank, defaultCurrencies } }; ;
        }

        Dictionary<string, List<string>> banksCurrenciesDictionary = new();
        for (int i = 0; i < args.Length; i += 2) {
            string bank = args[i];
            string[] currencies = args[i + 1].Split(';');

            if (banksCurrenciesDictionary.TryGetValue(bank, out var existingCurrenciesList)) {
                existingCurrenciesList.AddRange(currencies);
            } else {
                banksCurrenciesDictionary[bank] = new List<string>(currencies);
            }
        }

        return banksCurrenciesDictionary.ToDictionary(kvp => kvp.Key, kvp => (IEnumerable<string>)kvp.Value);
    }
}
