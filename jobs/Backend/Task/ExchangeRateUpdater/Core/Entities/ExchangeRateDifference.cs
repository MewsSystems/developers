using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Entities
{
    public class ExchangeRateDifference
    {
        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal FirstDateRate { get; }

        public decimal SecondDateRate { get; }

        public decimal Difference { get; }
        public decimal PercentageDifference { get; }


        public ExchangeRateDifference
        (
            Currency sourceCurrency,
            Currency targetCurrency,
            decimal firstDateRate,
            decimal secondDateRate,
            decimal difference,
            decimal percentageDifference
        )
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            FirstDateRate = firstDateRate;
            SecondDateRate = secondDateRate;
            Difference = difference;
            PercentageDifference = percentageDifference;
        }


        public override string ToString()
        {
            string percentageSign = PercentageDifference >= 0 ? "+" : "";
            return $"{SourceCurrency}/{TargetCurrency} - {FirstDateRate} -> {SecondDateRate} = {Difference} | {percentageSign}{PercentageDifference}%";
        }
    }
}
