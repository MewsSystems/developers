using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    private static readonly Currency Czk = new("CZK");

    private static readonly string[] AlwaysPresentCurrencyCodes = { "EUR", "USD" };
    private static readonly string[] InvalidCurrencyCodes = { "AAA", "BBB", "XYZ" };

    private IReadOnlyCollection<Currency> _currencies = null!;
    private IReadOnlyCollection<ExchangeRate> _exchangeRates = null!;

    [Params(10, 31)]
    public int NumOfLookUpCurrencies { get; set; }

    [Params(31, 160)]
    public int NumOfRates { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Randomizer.Seed = new Random(5552368);
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
        foreach (var currency in expectedCurrencies)
        {
            foreach (var exchangeRate in exchangeRates)
            {
                if (exchangeRate.SourceCurrency.Code == currency.Code)
                {
                    rates.Add(exchangeRate);
                    break;
                }
            }
        }

        return rates;
    }

    [Benchmark]
    public IReadOnlyCollection<ExchangeRate> LookupLoopCurrenciesThenRatesWithListSpan()
    {
        var expectedCurrencies = _currencies;
        var exchangeRates = (List<ExchangeRate>)_exchangeRates;
        var exchangeRatesSpan = CollectionsMarshal.AsSpan(exchangeRates);

        var rates = new List<ExchangeRate>(exchangeRatesSpan.Length);
        foreach (var currency in expectedCurrencies)
        {
            foreach (var exchangeRate in exchangeRatesSpan)
            {
                if (exchangeRate.SourceCurrency.Code == currency.Code)
                {
                    rates.Add(exchangeRate);
                    break;
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

    [Benchmark]
    [SkipLocalsInit]
    public unsafe IReadOnlyCollection<ExchangeRate> LookupWithSpanIndex()
    {
        var expectedCurrencies = _currencies;
        var exchangeRates = (List<ExchangeRate>)_exchangeRates;
        var exchangeRatesSpan = CollectionsMarshal.AsSpan(exchangeRates);

        // build lookup array where each cell points to position in original collection
        //
        //         [ 0        | 1        | 2        ]
        //  rates: [ EUR      | USD      | GBP      ]
        // lookup: [ EUR -> 0 | USD -> 1 | GBP -> 2 ]
        Span<CurrencyPosition> lookup = stackalloc CurrencyPosition[exchangeRatesSpan.Length];

        var idx = 0;
        foreach (var exchangeRate in exchangeRatesSpan)
        {
            lookup[idx] = new CurrencyPosition(idx, exchangeRate.SourceCurrency.Code);
            ++idx;
        }

        // for each expected currency...
        var rates = new List<ExchangeRate>(expectedCurrencies.Count);
        foreach (var currency in expectedCurrencies)
        {
            // for each rate lookup item...
            for (var i = 0; i < lookup.Length; i++)
            {
                // if currencies match, get rate from original collection and add it to result list
                if (lookup[i].Matches(currency.Code))
                {
                    var originalPosition = lookup[i].Position;
                    rates.Add(exchangeRatesSpan[originalPosition]);
                    break;
                }
            }
        }

        return rates;
    }

    [Benchmark]
    [SkipLocalsInit]
    public IReadOnlyCollection<ExchangeRate> LookupWithSpanBinarySearch()
    {
        var expectedCurrencies = _currencies;
        var exchangeRates = (List<ExchangeRate>)_exchangeRates;
        var exchangeRatesSpan = CollectionsMarshal.AsSpan(exchangeRates);

        // build lookup array where each cell points to position in original collection
        //
        //                [ 0        | 1        | 2        ]
        //         rates: [ EUR      | USD      | GBP      ]
        // sorted lookup: [ EUR -> 0 | GBP -> 2 | USD -> 1 ]
        Span<CurrencyPosition> lookup = stackalloc CurrencyPosition[exchangeRatesSpan.Length];

        var idx = 0;
        foreach (var exchangeRate in exchangeRatesSpan)
        {
            lookup[idx] = new CurrencyPosition(idx, exchangeRate.SourceCurrency.Code);
            ++idx;
        }

        lookup.Sort(CurrencyPosition.Comparer);

        var rates = new List<ExchangeRate>(expectedCurrencies.Count);
        foreach (var currency in expectedCurrencies)
        {
            var target = new CurrencyPosition(-1, currency.Code);
            var lookupPosition = lookup.BinarySearch(target, CurrencyPosition.Comparer);
            if (lookupPosition >= 0)
            {
                var originalPosition = lookup[lookupPosition].Position;
                rates.Add(exchangeRatesSpan[originalPosition]);
            }
        }

        return rates;
    }

    private List<Currency> GenerateLookupCurrencies(Faker faker)
    {
        var lookupCurrencies = new HashSet<string>(AlwaysPresentCurrencyCodes.Concat(InvalidCurrencyCodes));
        while (lookupCurrencies.Count < NumOfLookUpCurrencies)
        {
            lookupCurrencies.Add(faker.Finance.Currency().Code);
        }

        return lookupCurrencies
            .Select(c => new Currency(c))
            .ToList();
    }

    private List<ExchangeRate> GenerateExchangeRates(Faker faker)
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
            .ToList();
    }

    private readonly struct CurrencyPosition
    {
        public static readonly CurrencyPositionComparer Comparer = new();

        private readonly int _encoded;
        private readonly int _index;

        public CurrencyPosition(int index, string code)
        {
            _index = index;
            _encoded = EncodeCurrencyCode(code);
        }

        public int Position => _index;

        public bool Matches(string code) =>
            _encoded == EncodeCurrencyCode(code);

        private static int EncodeCurrencyCode(string code) =>
            (code[0] << 16) | (code[1] << 8) | code[2];

        public class CurrencyPositionComparer : IComparer<CurrencyPosition>
        {
            public int Compare(CurrencyPosition a, CurrencyPosition b) =>
                a._encoded.CompareTo(b._encoded);
        }
    }
}