using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRateProvider.Model
{
    [Serializable]
    [JsonObject]
    public abstract class RootObject
    {
        public string Updated { get; set; }
        public string TableNameHeader { get; set; }
        public string TableGraphHeader { get; set; }
        public List<string> TableDynamicHeaders { get; set; }
        public List<TableEntry> TableEntries { get; set; }
    }
}