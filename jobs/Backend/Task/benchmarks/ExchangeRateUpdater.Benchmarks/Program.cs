using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Bogus;
using ExchangeRateUpdater;

var config = DefaultConfig.Instance;
var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

[MemoryDiagnoser]
public class CurrencyLookupBenchmarks
{
    private static readonly Currency Czk = new Currency("CZK");

    private static readonly string[] AlwaysPresentCurrencyCodes = { "EUR", "USD" };
    private static readonly string[] InvalidCurrencyCodes = { "AAA", "BBB", "XYZ" };
    
    private IReadOnlyCollection<Currency> _currencies = null!;
    private IReadOnlyCollection<ExchangeRate> _exchangeRates = null!;
    
    [Params(4, 10, 31)]
    public int NumOfLookUpCurrencies { get; set; }
    
    [Params(31, 160)]
    public int NumOfRates { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Randomizer.Seed = new Random(42);
        var faker = new Faker();

        _currencies = GenerateLookupCurrencies(faker);
        _exchangeRates = GenerateExchangeRates(faker);
    }
    
    [Benchmark(Baseline = true)]
    public IReadOnlyCollection<ExchangeRate> LookupLoopCurrenciesThenRates()
    {
        var expectedCurrencies = _currencies;
        var exchangeRates = _exchangeRates;
        
        var rates = new List<ExchangeRate>(expectedCurrencies.Count);
        foreach (var exchangeRate in exchangeRates)
        {
            foreach (var currency in expectedCurrencies)
            {
                if (exchangeRate.SourceCurrency.Code == currency.Code)
                {
                    rates.Add(exchangeRate);
                }
            }
        }
        
        return rates;
    }

    [Benchmark]
    public IReadOnlyCollection<ExchangeRate> LookupWithHashSet()
    {
        var expectedCurrencies = _currencies;
        var exchangeRates = _exchangeRates;

        var expectedCurrenciesLookup = new HashSet<string>(expectedCurrencies.Select(c => c.Code));

        var rates = new List<ExchangeRate>(expectedCurrencies.Count);
        foreach (var exchangeRate in exchangeRates)
        {
            if (expectedCurrenciesLookup.Contains(exchangeRate.SourceCurrency.Code))
            {
                rates.Add(exchangeRate);
            }
        }

        return rates;
    }
    
    [Benchmark]
    public IReadOnlyCollection<ExchangeRate> LookupWithDictionary()
    {
        var expectedCurrencies = _currencies;
        var exchangeRates = _exchangeRates;

        var ratesByCurrency = exchangeRates.ToDictionary(r => r.SourceCurrency.Code);

        var rates = new List<ExchangeRate>(expectedCurrencies.Count);
        foreach (var currency in expectedCurrencies)
        {
            if (ratesByCurrency.TryGetValue(currency.Code, out var rate))
            {
                rates.Add(rate);
            }
        }

        return rates;
    }    

    private Currency[] GenerateLookupCurrencies(Faker faker)
    {
        var lookupCurrencies = new HashSet<string>(
            AlwaysPresentCurrencyCodes.Concat(
                faker.PickRandom(
                    items: InvalidCurrencyCodes,
                    amountToPick: faker.Random.Int(1, InvalidCurrencyCodes.Length))));

        while (lookupCurrencies.Count < NumOfLookUpCurrencies)
        {
            lookupCurrencies.Add(faker.Finance.Currency().Code);
        }

        return lookupCurrencies
            .Select(c => new Currency(c))
            .ToArray();
    }
    
    private ExchangeRate[] GenerateExchangeRates(Faker faker)
    {
        var rates = new HashSet<string>(AlwaysPresentCurrencyCodes);
        while (rates.Count < NumOfRates)
        {
            rates.Add(faker.Finance.Currency().Code);
        }

        return rates
            .Select(
                r => new ExchangeRate(
                    sourceCurrency: new Currency(r),
                    targetCurrency: Czk,
                    value: faker.Random.Decimal()))
            .ToArray();
    }
}