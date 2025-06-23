using ExchangeRateService.Client.Model.CNB;
using Refit;

namespace ExchangeRateService.Client;

public interface ICNBRefitClient
{
    [Get("/cnbapi/exrates/daily")]
    Task<ExratesDailyResponse> GetExratesDailyRates([Query(Format = "yyyy-MM-dd")] DateTime date, [Query] CNBLanguage lang = CNBLanguage.EN);
    
    [Get("/cnbapi/fxrates/daily-month")]
    Task<FXRatesDailyMonthResponse> GetFXRatesDailyMonthRates([Query(Format = "yyyy-MM")] DateTime yearMonth, [Query] CNBLanguage lang = CNBLanguage.EN);
}