using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExchangeRateUpdater.ViewModels
{
    static class Currencies
    {
        public static readonly Currency USD = new Currency(nameof(USD));
        public static readonly Currency EUR = new Currency(nameof(EUR));
        public static readonly Currency CZK = new Currency(nameof(CZK));
        public static readonly Currency JPY = new Currency(nameof(JPY));
        public static readonly Currency KES = new Currency(nameof(KES));
        public static readonly Currency RUB = new Currency(nameof(RUB));
        public static readonly Currency THB = new Currency(nameof(THB));
        public static readonly Currency TRY = new Currency(nameof(TRY));
        public static readonly Currency XYZ = new Currency(nameof(XYZ));

        public static readonly IReadOnlyCollection<Currency> All =
            new ReadOnlyCollection<Currency>(new[] { USD, EUR, CZK, JPY, KES, RUB, THB, TRY, XYZ });
    }
}
