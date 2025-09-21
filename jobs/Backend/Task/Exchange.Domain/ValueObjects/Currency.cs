namespace Exchange.Domain.ValueObjects;

public record Currency
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; init; }

    public string Country { get; init; }

    public string Name { get; init; }

    private Currency(string Code, string Country, string Name)
    {
        this.Code = Code;
        this.Country = Country;
        this.Name = Name;
    }

    public static readonly Currency AUD = new("AUD", "Australia", "dollar");
    public static readonly Currency BRL = new("BRL", "Brazil", "real");
    public static readonly Currency BGN = new("BGN", "Bulgaria", "lev");
    public static readonly Currency CAD = new("CAD", "Canada", "dollar");
    public static readonly Currency CNY = new("CNY", "China", "renminbi");
    public static readonly Currency CZK = new("CZK", "Czech Republic", "koruna");
    public static readonly Currency HRK = new("HRK", "Croatia", "kuna");
    public static readonly Currency DKK = new("DKK", "Denmark", "krone");
    public static readonly Currency EUR = new("EUR", "EMU", "euro");
    public static readonly Currency HKD = new("HKD", "Hongkong", "dollar");
    public static readonly Currency HUF = new("HUF", "Hungary", "forint");
    public static readonly Currency ISK = new("ISK", "Iceland", "krona");
    public static readonly Currency XDR = new("XDR", "IMF", "SDR");
    public static readonly Currency INR = new("INR", "India", "rupee");
    public static readonly Currency IDR = new("IDR", "Indonesia", "rupiah");
    public static readonly Currency ILS = new("ILS", "Israel", "new shekel");
    public static readonly Currency JPY = new("JPY", "Japan", "yen");
    public static readonly Currency MYR = new("MYR", "Malaysia", "ringgit");
    public static readonly Currency MXN = new("MXN", "Mexico", "peso");
    public static readonly Currency NZD = new("NZD", "New Zealand", "dollar");
    public static readonly Currency NOK = new("NOK", "Norway", "krone");
    public static readonly Currency PHP = new("PHP", "Philippines", "peso");
    public static readonly Currency PLN = new("PLN", "Poland", "zloty");
    public static readonly Currency RON = new("RON", "Romania", "leu");
    public static readonly Currency RUB = new("RUB", "Russia", "rouble");
    public static readonly Currency SGD = new("SGD", "Singapore", "dollar");
    public static readonly Currency ZAR = new("ZAR", "South Africa", "rand");
    public static readonly Currency KES = new("KES", "Kenya", "shilling");
    public static readonly Currency KRW = new("KRW", "South Korea", "won");
    public static readonly Currency SEK = new("SEK", "Sweden", "krona");
    public static readonly Currency CHF = new("CHF", "Switzerland", "franc");
    public static readonly Currency THB = new("THB", "Thailand", "baht");
    public static readonly Currency TRY = new("TRY", "Turkey", "lira");
    public static readonly Currency GBP = new("GBP", "United Kingdom", "pound");
    public static readonly Currency USD = new("USD", "USA", "dollar");

    private static readonly Dictionary<string, Currency> Currencies = new(StringComparer.OrdinalIgnoreCase)
    {
        ["AUD"] = AUD,
        ["BRL"] = BRL,
        ["BGN"] = BGN,
        ["CAD"] = CAD,
        ["CNY"] = CNY,
        ["HRK"] = HRK,
        ["DKK"] = DKK,
        ["EUR"] = EUR,
        ["HKD"] = HKD,
        ["HUF"] = HUF,
        ["ISK"] = ISK,
        ["XDR"] = XDR,
        ["INR"] = INR,
        ["IDR"] = IDR,
        ["ILS"] = ILS,
        ["JPY"] = JPY,
        ["MYR"] = MYR,
        ["MXN"] = MXN,
        ["NZD"] = NZD,
        ["NOK"] = NOK,
        ["PHP"] = PHP,
        ["PLN"] = PLN,
        ["RON"] = RON,
        ["RUB"] = RUB,
        ["SGD"] = SGD,
        ["ZAR"] = ZAR,
        ["KRW"] = KRW,
        ["SEK"] = SEK,
        ["CHF"] = CHF,
        ["THB"] = THB,
        ["TRY"] = TRY,
        ["GBP"] = GBP,
        ["USD"] = USD,
        ["CZK"] = CZK
    };

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or whitespace.", nameof(code));

        if (Currencies.TryGetValue(code, out var currency))
            return currency;

        throw new ArgumentException($"Unsupported currency code: {code}", nameof(code));
    }
}