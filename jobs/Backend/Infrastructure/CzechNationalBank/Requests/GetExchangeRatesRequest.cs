using ApiClients.CzechNationalBank.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.CzechNationalBank.Requests;

public class GetExchangeRatesRequest : CsvRequest
{
    private readonly IOptions<CzechNationalBankApiOptions> _czechNationalBankApiOptions;

    public GetExchangeRatesRequest(IOptions<CzechNationalBankApiOptions> czechNationalBankApiOptions) : base(HttpMethod.Get)
    {
        _czechNationalBankApiOptions = czechNationalBankApiOptions;
    }

    public override void SetUri()
    {
        Uri = new Uri(_czechNationalBankApiOptions.Value.BaseUrl + _czechNationalBankApiOptions.Value.GetRates);
    }
}
