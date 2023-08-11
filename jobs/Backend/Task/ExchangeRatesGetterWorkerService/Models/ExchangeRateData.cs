using ExchangeRatesGetterWorkerService.Helpers;
using System.Globalization;

namespace ExchangeRatesGetterWorkerService.Models
{
    public class ExchangeRateData
    {
        public static ExchangeRateData CreateFromMainCurrencyCnbRate(Rate rate)
        {
            ExchangeRateData rateData = new ExchangeRateData();
            rateData.TargetCurrency = "CZK";
            rateData.SourceCurrency = rate.currencyCode;
            rateData.Value = Convert.ToDecimal(rate.rate);
            rateData.ValidFrom = DateTime.ParseExact(rate.validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddHours(14.5);

            DateTime validTill = rateData.ValidFrom.AddDays(1);
            while(!DateTimeHelper.IsWorkingDay(validTill))
            {
                validTill = validTill.AddDays(1);
            }

            rateData.ValidTill = validTill;

            return rateData;
        }

        public static ExchangeRateData CreateFromOtherCurrencyCnbRate(Rate rate)
        {
            ExchangeRateData rateData = new ExchangeRateData();
            rateData.TargetCurrency = "CZK";
            rateData.SourceCurrency = rate.currencyCode;
            rateData.Value = Convert.ToDecimal(rate.rate);

            DateTime validFor = DateTime.ParseExact(rate.validFor, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime temp = validFor.AddMonths(1);
            DateTime validFrom = new DateTime(temp.Year, temp.Month, 1);
            DateTime validTo = validFrom.AddMonths(1).AddMinutes(-1);

            rateData.ValidFrom = validFrom;
            rateData.ValidTill = validTo;

            return rateData;
        }
        public int Id { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Value { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }


    }
}
