using System;
using System.Collections.Generic;
using ExchangeRate.Provider.Base.Models;
using ExchangeRate.Provider.Cnb.Models;
using ExchangeRate.Provider.Cnb.Models.Configuration;
using ExchangeRate.Service.UnitTests.Enums;

namespace ExchangeRate.Service.UnitTests.Mock;

public static class MockData
{
    // TODO: Fix with C#11
    public const string ExchangeRateEnglishSampleMissingColumnHeader = @"
                    Australia|dollar|1|AUD|16.217
                    Brazil|real|1|BRL|4.510
                    Bulgaria|lev|1|BGN|12.643
                    Canada|dollar|1|CAD|18.107
                    China|renminbi|1|CNY|3.511
                    Croatia|kuna|1|HRK|3.284
                    Denmark|krone|1|DKK|3.324
                    EMU|euro|1|EUR|24.730
                    Hongkong|dollar|1|HKD|2.994
                    Hungary|forint|100|HUF|6.163
                    Iceland|krona|100|ISK|17.702
                    IMF|SDR|1|XDR|31.336
                    India|rupee|100|INR|30.016
                    Indonesia|rupiah|1000|IDR|1.583
                    Israel|new shekel|1|ILS|6.832
                    Japan|yen|100|JPY|17.389
                    Malaysia|ringgit|1|MYR|5.339
                    Mexico|peso|1|MXN|1.178
                    New Zealand|dollar|1|NZD|14.780
                    Norway|krone|1|NOK|2.370
                    Philippines|peso|100|PHP|42.760
                    Poland|zloty|1|PLN|5.262
                    Romania|leu|1|RON|5.000
                    Singapore|dollar|1|SGD|16.916
                    South Africa|rand|1|ZAR|1.479
                    South Korea|won|100|KRW|1.813
                    Sweden|krona|1|SEK|2.312
                    Switzerland|franc|1|CHF|24.552
                    Thailand|baht|100|THB|66.191
                    Turkey|lira|1|TRY|1.353
                    United Kingdom|pound|1|GBP|28.829
                    USA|dollar|1|USD|23.501";

    // TODO: Fix with C#11
    public const string ExchangeRateCzechSampleMissingColumnHeader = @"
                    Australia|dollar|1|AUD|16,217
                    Brazil|real|1|BRL|4,510
                    Bulgaria|lev|1|BGN|12,643
                    Canada|dollar|1|CAD|18,107
                    China|renminbi|1|CNY|3,511
                    Croatia|kuna|1|HRK|3,284
                    Denmark|krone|1|DKK|3,324
                    EMU|euro|1|EUR|24,730
                    Hongkong|dollar|1|HKD|2,994
                    Hungary|forint|100|HUF|6,163
                    Iceland|krona|100|ISK|17,702
                    IMF|SDR|1|XDR|31,336
                    India|rupee|100|INR|30,016
                    Indonesia|rupiah|1000|IDR|1,583
                    Israel|new shekel|1|ILS|6,832
                    Japan|yen|100|JPY|17,389
                    Malaysia|ringgit|1|MYR|5,339
                    Mexico|peso|1|MXN|1,178
                    New Zealand|dollar|1|NZD|14,780
                    Norway|krone|1|NOK|2,370
                    Philippines|peso|100|PHP|42,760
                    Poland|zloty|1|PLN|5,262
                    Romania|leu|1|RON|5,000
                    Singapore|dollar|1|SGD|16,916
                    South Africa|rand|1|ZAR|1,479
                    South Korea|won|100|KRW|1,813
                    Sweden|krona|1|SEK|2,312
                    Switzerland|franc|1|CHF|24,552
                    Thailand|baht|100|THB|66,191
                    Turkey|lira|1|TRY|1,353
                    United Kingdom|pound|1|GBP|28,829
                    USA|dollar|1|USD|23,501";

    // TODO: Fix with C#11
    public const string ExchangeRateEnglishSampleMissingDateHeader = @"Country|Currency|Amount|Code|Rate
                    Australia|dollar|1|AUD|16.217
                    Brazil|real|1|BRL|4.510
                    Bulgaria|lev|1|BGN|12.643
                    Canada|dollar|1|CAD|18.107
                    China|renminbi|1|CNY|3.511
                    Croatia|kuna|1|HRK|3.284
                    Denmark|krone|1|DKK|3.324
                    EMU|euro|1|EUR|24.730
                    Hongkong|dollar|1|HKD|2.994
                    Hungary|forint|100|HUF|6.163
                    Iceland|krona|100|ISK|17.702
                    IMF|SDR|1|XDR|31.336
                    India|rupee|100|INR|30.016
                    Indonesia|rupiah|1000|IDR|1.583
                    Israel|new shekel|1|ILS|6.832
                    Japan|yen|100|JPY|17.389
                    Malaysia|ringgit|1|MYR|5.339
                    Mexico|peso|1|MXN|1.178
                    New Zealand|dollar|1|NZD|14.780
                    Norway|krone|1|NOK|2.370
                    Philippines|peso|100|PHP|42.760
                    Poland|zloty|1|PLN|5.262
                    Romania|leu|1|RON|5.000
                    Singapore|dollar|1|SGD|16.916
                    South Africa|rand|1|ZAR|1.479
                    South Korea|won|100|KRW|1.813
                    Sweden|krona|1|SEK|2.312
                    Switzerland|franc|1|CHF|24.552
                    Thailand|baht|100|THB|66.191
                    Turkey|lira|1|TRY|1.353
                    United Kingdom|pound|1|GBP|28.829
                    USA|dollar|1|USD|23.501";

    // TODO: Fix with C#11
    public const string ExchangeRateCzechSampleMissingDateHeader = @"Country|Currency|Amount|Code|Rate
                    Australia|dollar|1|AUD|16,217
                    Brazil|real|1|BRL|4,510
                    Bulgaria|lev|1|BGN|12,643
                    Canada|dollar|1|CAD|18,107
                    China|renminbi|1|CNY|3,511
                    Croatia|kuna|1|HRK|3,284
                    Denmark|krone|1|DKK|3,324
                    EMU|euro|1|EUR|24,730
                    Hongkong|dollar|1|HKD|2,994
                    Hungary|forint|100|HUF|6,163
                    Iceland|krona|100|ISK|17,702
                    IMF|SDR|1|XDR|31,336
                    India|rupee|100|INR|30,016
                    Indonesia|rupiah|1000|IDR|1,583
                    Israel|new shekel|1|ILS|6,832
                    Japan|yen|100|JPY|17,389
                    Malaysia|ringgit|1|MYR|5,339
                    Mexico|peso|1|MXN|1,178
                    New Zealand|dollar|1|NZD|14,780
                    Norway|krone|1|NOK|2,370
                    Philippines|peso|100|PHP|42,760
                    Poland|zloty|1|PLN|5,262
                    Romania|leu|1|RON|5,000
                    Singapore|dollar|1|SGD|16,916
                    South Africa|rand|1|ZAR|1,479
                    South Korea|won|100|KRW|1,813
                    Sweden|krona|1|SEK|2,312
                    Switzerland|franc|1|CHF|24,552
                    Thailand|baht|100|THB|66,191
                    Turkey|lira|1|TRY|1,353
                    United Kingdom|pound|1|GBP|28,829
                    USA|dollar|1|USD|23,501";

    public static readonly List<CnbExchangeRate> ExchangeRatesEnglishSampleList = new()
    {
        new CnbExchangeRate("Australia", "dollar", 1, "AUD", 16.217m),
        new CnbExchangeRate("Brazil", "real", 1, "BRL", 4.510m),
        new CnbExchangeRate("Bulgaria", "lev", 1, "BGN", 12.643m),
        new CnbExchangeRate("USA", "dollar", 1, "USD", 23.501m),
        new CnbExchangeRate("EMU", "euro", 1, "EUR", 24.730m)
    };

    public static string ExchangeRateEnglishSample(CnbSourceLanguage language)
    {
        return language switch
        {
            CnbSourceLanguage.English => @"24 Jun 2022 #123
                    Country|Currency|Amount|Code|Rate
                    EMU|euro|1|EUR|24.730
                    Singapore|dollar|1|SGD|16.916
                    USA|dollar|1|USD|23.501",
            CnbSourceLanguage.Czech => @"24 Jun 2022 #123
                    země|měna|množství|kód|kurz
                    EMU|euro|1|EUR|24,730
                    Singapur|dollar|1|SGD|16,916
                    USA|dollar|1|USD|23,501",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }

    public static CnbProviderConfiguration CnbProviderConfigurationSample(CnbSourceLanguage language)
    {
        return new CnbProviderConfiguration
        {
            Cache = new CacheConfiguration(true, "Central Europe Standard Time"),
            Connection = GetConnectionConfiguration(language),
            Currencies = new List<string>
            {
                "USD",
                "EUR",
                "CZK",
                "JPY",
                "KES",
                "RUB",
                "THB",
                "TRY",
                "XYZ"
            },
            DataSourceFields = GetCnbDataSourceFieldsConfiguration(language)
        };
    }

    private static ConnectionConfiguration GetConnectionConfiguration(CnbSourceLanguage language)
    {
        return language switch
        {
            CnbSourceLanguage.English => new ConnectionConfiguration(
                "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"),
            CnbSourceLanguage.Czech => new ConnectionConfiguration(
                "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt"),
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }

    private static CnbDataSourceFieldsConfiguration GetCnbDataSourceFieldsConfiguration(CnbSourceLanguage language)
    {
        return language switch
        {
            CnbSourceLanguage.English => new CnbDataSourceFieldsConfiguration
            {
                Amount = "Amount",
                Code = "Code",
                Country = "Country",
                Currency = "Currency",
                Rate = "Rate",
                Separator = "|"
            },
            CnbSourceLanguage.Czech => new CnbDataSourceFieldsConfiguration
            {
                Amount = "množství",
                Code = "kód",
                Country = "země",
                Currency = "měna",
                Rate = "kurz",
                Separator = "|"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }
}