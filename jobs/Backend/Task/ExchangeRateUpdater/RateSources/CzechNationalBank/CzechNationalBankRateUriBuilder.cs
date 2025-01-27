using System;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public interface ICzechNationalBankRateUriBuilder
{
    Uri BuildMainSourceUri(DateOnly targetDate);
    Uri BuildSecondarySourceUri(DateOnly targetDate);
}

public class CzechNationalBankRateUriBuilder : ICzechNationalBankRateUriBuilder
{
    private static string DateParameterFormat = "dd.MM.yyyy";

    private readonly Uri _mainSourceUri;
    private readonly Uri _secondarySourceUri;
    public CzechNationalBankRateUriBuilder(IOptions<CzechNationalBankSourceOptions> sourceUriConfig)
    {
        _mainSourceUri = sourceUriConfig.Value.MainDataSourceUrl;
        _secondarySourceUri = sourceUriConfig.Value.SecondaryDataSourceUrl;
    }
    public Uri BuildMainSourceUri(DateOnly targetDate)
    {
        var dateParam = $"date={targetDate.ToString(DateParameterFormat)}";
        var targetUrlBuilder = new UriBuilder(_mainSourceUri);
        targetUrlBuilder.Query = dateParam;
        return targetUrlBuilder.Uri;
    }

    public Uri BuildSecondarySourceUri(DateOnly targetDate)
    {
        var dateParams = $"year={targetDate.Year}&month={targetDate.Month}";
        var targetUrlBuilder = new UriBuilder(_secondarySourceUri);
        targetUrlBuilder.Query = dateParams;
        return targetUrlBuilder.Uri;
    }
}