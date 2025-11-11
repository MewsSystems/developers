using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IExchangeRateConverter<T>
    {
        public Task<List<ExchangeRateDTO>> Convert(T response);
    }
}
