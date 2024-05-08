namespace ExchangeRateUpdater.Integration.Tests.Contexts;

public class HttpScenarioContext
{
    private const string _forwardedForHeader = "X-Forwarded-For";

    public readonly HttpClient Client = TestHost.Client;

    public HttpScenarioContext()
    {
        Client = TestHost.Client;
        SetForwardedForHeader("203.0.113.195,2001:db8:85a3:8d3:1319:8a2e:370:7348,198.51.100.178");
    }

    public void SetForwardedForHeader(string forwardedFor)
    {
        Client.DefaultRequestHeaders.Remove(_forwardedForHeader);
        Client.DefaultRequestHeaders.Add(_forwardedForHeader, forwardedFor);
    }
}
