using Data;

namespace Infrastructure.CzechNationalBank;

public class CzechNationalBankHttpApiClient : BaseHttpApiClient, ICzechNationalBankHttpApiClient
{

    public CzechNationalBankHttpApiClient(IHttpClientFactory httpClientFactory, ILog<BaseHttpApiClient> logger) : base(httpClientFactory, logger)
    {
    }

    public override string HttpClientName => "CzechNationalBankApi";
}
