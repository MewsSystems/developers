using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Entities.Json
{
    public class ExchangeRateModel : IGenericEntity
    {
        public string result { get; set; }
        public string base_code { get; set; }
        public Dictionary<string, decimal> conversion_rates { get; set; }

        public IEnumerable<GenericRate> ToGenericEntity()
        {
            return conversion_rates
                .Select(item =>
                    new GenericRate(item.Key, item.Value))
                .ToList();
        }
    }
}