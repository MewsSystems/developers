namespace ExchangeRateUpdater.Utils
{
    public static class CnbConstants
    {
        private static string _BASE_URL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt;date=";
        public static string BASE_URL { get => _BASE_URL; }

        private static char _EXPECTED_FILE_SEPARATOR = '|';
        public static char EXPECTED_FILE_SEPARATOR { get => _EXPECTED_FILE_SEPARATOR; }

        private static int _EXPECTED_ROW_SIZE = 5;
        public static int EXPECTED_ROW_SIZE { get => _EXPECTED_ROW_SIZE; }

        private static int _LINES_TO_SKIP = 2;
        public static int LINES_TO_SKIP { get => _LINES_TO_SKIP; }
    }
}
