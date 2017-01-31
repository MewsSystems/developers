using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRateProvider.Model
{
    [Serializable]
    [JsonObject]
    public class TableEntry
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int ConversionFactor { get; set; }

        public List<decimal> Values { get; set; }
        public string GraphUrl { get; set; }
    }
}