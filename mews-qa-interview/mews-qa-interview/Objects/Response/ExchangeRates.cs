using System.Collections.Generic;

namespace MewsQaInterview.Objects.Response
{

    public class GetExhangeRates
    {
        public List<Exchangerate> ExchangeRates { get; set; }
    }

    public class Exchangerate
    {
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public float Value { get; set; }
    }

}
