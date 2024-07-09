using ExchangeRateUpdater.Integration.Tests.Contexts;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Steps.Common;

[Binding]
public class HttpSteps(ScenarioContext scenarioContext, HttpScenarioContext httpScenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;
    private readonly HttpScenarioContext _httpScenarioContext = httpScenarioContext;

    [When(@"I call the '([^']*)' endpoint")]
    public async Task WhenICallTheEndpoint(string endpoint)
    {
        _scenarioContext.Set(await _httpScenarioContext.Client.GetAsync(endpoint));
    }

    [Then(@"HttpStatus (.*) is returned")]
    public void ThenHttpStatusIsReturned(HttpStatusCode httpStatus)
    {
        _scenarioContext.Get<HttpResponseMessage>().StatusCode.Should().Be(httpStatus);
    }

    [Then(@"response is")]
    public async Task ThenResponseIs(string expectedJson)
    {
        string actualJson = await GetResponseAsStringAsync();
        AreJObjectEqual(expectedJson, actualJson);
    }

    [Then(@"response is a string like '([^']*)'")]
    public async Task ThenResponseIsAStringLike(string expected)
    {
        string actual = await GetResponseAsStringAsync();
        actual.Should().Be(expected);
    }

    private static bool AreJObjectEqual(string expectedJson, string actualJson)
    {
        string actualIndentedJson = GetIndentedJson(actualJson);
        string expectedIndentedJson = GetIndentedJson(expectedJson);

        var actual = JObject.Parse(actualIndentedJson);
        var expected = JObject.Parse(expectedIndentedJson);

        actual.Should().BeEquivalentTo(expected);

        return true;
    }

    private static string GetIndentedJson(string json)
    {
        dynamic? parsedJson = JsonConvert.DeserializeObject<dynamic>(json);
        return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
    }

    private async Task<string> GetResponseAsStringAsync() => (await _scenarioContext.Get<HttpResponseMessage>().GetContentAsStringAsync())!;
}
