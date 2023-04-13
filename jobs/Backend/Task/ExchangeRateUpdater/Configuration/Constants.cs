using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Configuration
{
    public static class Constants
    {
        public const string DateFormat = "dd.MM.yyyy";

        #region CzechExchangeRateParser

        public const string CNBParserColumnSeparator = "|";
        public const int CNBParserRowToSkip = 2;

        #endregion
    }
}
