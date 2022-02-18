﻿using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateDeserializer
    {
        ExchangeRate Deserialize(string serializedExchangeRate);
    }
}
