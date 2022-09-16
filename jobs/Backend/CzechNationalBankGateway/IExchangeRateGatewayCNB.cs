using CzechNationalBankAPI.Model;
using Model;
using Model.Entities;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace CzechNationalBankGateway
{
    public interface IExchangeRateGatewayCNB
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates();
    }
}