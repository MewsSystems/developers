using Microsoft.AspNetCore.Mvc.Testing;

namespace ExchangeRateService.IntegrationTests.Helpers;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
}