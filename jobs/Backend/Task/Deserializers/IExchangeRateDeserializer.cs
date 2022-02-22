using ExchangeRateUpdater.DTO;

namespace ExchangeRateUpdater.Deserializers
{
    public interface IExchangeRateDeserializer
    {
        ExchangeRate Deserialize(string serializedExchangeRate);
    }
}
