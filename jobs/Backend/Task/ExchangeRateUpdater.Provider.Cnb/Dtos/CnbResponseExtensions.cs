using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Provider.Cnb.Dtos
{
    public static class CnbResponseExtensions
    {
        public static DateOnly? GetValidForDate(this CnbResponse? resp)
        {
            var validForDate = resp?.Rates?.FirstOrDefault()?.ValidFor;
            
            if (validForDate == null)
                return null;
            
            return DateOnly.ParseExact(
                validForDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
