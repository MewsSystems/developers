using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.PZ.Models
{
    public class MyCurrency : Currency
    {
        public MyCurrency(string code, decimal Exchangerate) : base(code)
        {
            ExchangeRate = Exchangerate;
        }

        #region Properties
        public decimal ExchangeRate { get; }
        #endregion

        public static MyCurrency GetMyCurrency(string stringData)
        {
            var temp = stringData.Split('|').ToList();
            temp.RemoveRange(0, 2);
            int count = 0;
            int.TryParse(temp[0], out count);
            string code = temp[1];
            decimal exchangeRate = 0;
            decimal.TryParse(ToRegionalNumber(temp.Last()), out exchangeRate);
            return new MyCurrency(code, exchangeRate / count);

        }

        static string ToRegionalNumber(string stringNumber)
        {
            string regionalNb = stringNumber.Replace(",", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            regionalNb = regionalNb.Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);

            return regionalNb;
        }
    }
}
