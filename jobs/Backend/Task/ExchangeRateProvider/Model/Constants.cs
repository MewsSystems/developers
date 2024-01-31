using System;

namespace ExchangeRateUpdater.Model
{
    public static class Constants
    {
        public static class Currencies
        {

            static readonly Lazy<Currency> _czk = new(() => new Currency(nameof(CZK)));
            static readonly Lazy<Currency> _usd = new(() => new Currency(nameof(USD)));

            static readonly Lazy<Currency> _eur = new(() => new Currency(nameof(EUR)));
            static readonly Lazy<Currency> _jpy = new(() => new Currency(nameof(JPY)));
            static readonly Lazy<Currency> _aud = new(() => new Currency(nameof(AUD)));
            static readonly Lazy<Currency> _php = new(() => new Currency(nameof(PHP)));
            static readonly Lazy<Currency> _thb = new(() => new Currency(nameof(THB)));

            public static Currency CZK => _czk.Value;

            public static Currency USD => _usd.Value;

            public static Currency EUR => _eur.Value;

            public static Currency JPY => _jpy.Value;

            public static Currency AUD => _aud.Value;

            public static Currency PHP => _php.Value;

            public static Currency THB => _thb.Value;
        }
    }
}
