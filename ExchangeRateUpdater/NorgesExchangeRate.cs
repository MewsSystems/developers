using System.Runtime.Serialization;

namespace ExchangeRateUpdater
{
    [DataContract]
    public class NorgesExchangeRate
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string TitleNO { get; set; }

        [DataMember]
        public string TitleEN { get; set; }

        [DataMember]
        public decimal CurrentValue { get; set; }
    }
}