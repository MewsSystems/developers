using System;
namespace Core.Interfaces
{
    public interface IDeserializationService
    {
        T Deserialize<T>(string value);
    }
}
