using System;

namespace Mews.Integrations.Cnb.Contracts.Models
{
    public record ExchangeRate
    {
        public Currency SourceCurrency { get; init; } = null!;
        public Currency TargetCurrency { get; init;} = null!;
        public decimal Value { get; init;}
        
        public DateTimeOffset ValidFor { get; init;}

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
