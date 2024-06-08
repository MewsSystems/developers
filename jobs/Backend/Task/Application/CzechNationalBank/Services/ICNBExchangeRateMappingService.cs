using Application.Common.Models;
using Application.CzechNationalBank.ApiClient.Dtos;

namespace Application.CzechNationalBank.Mappings
{
    public interface ICNBExchangeRateMappingService
    {
        IList<ExchangeRate> ConvertToExchangeRates(IEnumerable<CNBExRateDailyRestDto> dtos);
    }
}