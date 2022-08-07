namespace CurrencyExchangeService.Interfaces
{
    public interface ISerializationHelper<T>
    {
        T Deserialize(string value);
        string Serialize(T value);
    }
}
