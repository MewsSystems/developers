using ExchangeRateUpdater.Dtos;

namespace ExchangeRateUpdater.Tests.Constants
{
    public static class ExchangeRateConstants
    {
        public static Currency Currency1 = new("USD");
        public static Currency Currency2 = new ("ABC");
        public static Currency Currency3 = new ("EUR");
        public static Currency Currency4 = new ("JPY");
        public static Currency Currency5 = new ("XYZ");
        public static Currency Currency6 = new ("CZK");
        
        public const int Amount1 = 0;
        public const int Amount2 = 1;
        public const int Amount3 = 100;
        
        public const decimal Rate1 = 11.222m;
        public const decimal Rate2 = 22.333m;
        public const decimal Rate3 = 33.444m;
        public const decimal Rate4 = 44.555m;
        public const decimal Rate5 = 55.666m;
        
    }
}