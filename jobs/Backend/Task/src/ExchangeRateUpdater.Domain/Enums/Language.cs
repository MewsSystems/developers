using System.Runtime.Serialization;

namespace ExchangeRateUpdater.Domain.Enums
{
    public enum Language
    {
        [EnumMember(Value = "CZ")]
        CZ,
        [EnumMember(Value = "EN")]
        EN
    }
}
