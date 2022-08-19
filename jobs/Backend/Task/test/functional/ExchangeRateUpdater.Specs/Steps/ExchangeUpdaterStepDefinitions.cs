using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeRateApiServiceClient;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace ExchangeRateUpdater.Specs.Steps
{
    [Binding]
    public sealed class ExchangeUpdaterStepDefinitions
    {
        [Given(@"exchange rate for (.*) / (.*) is (.*)")]
        public void GivenExchangeRateIs(string sourceCurrency, string targetCurrency, decimal rate)
        {
            var getExchangeRatesResponse = TestData.GetExchangeRatesResponses.ContainsKey(sourceCurrency) ? 
                TestData.GetExchangeRatesResponses[sourceCurrency] :
                null;
            
            if (getExchangeRatesResponse == null)
            {
                getExchangeRatesResponse = new GetExchangeRatesResponse()
                {
                    Rates = new Dictionary<string, decimal>()
                };
                TestData.GetExchangeRatesResponses.Add(sourceCurrency, getExchangeRatesResponse);
            }

            getExchangeRatesResponse!.Rates![targetCurrency] = rate;
        }

        [When(@"I run the ExchangeUpdater")]
        public async Task WhenIRunTheExchangeUpdater()
        {
            ArrangeWiremock();
            await RunMain();
        }

        private void ArrangeWiremock()
        {
            WireMock.Server
                .Given(Request.Create().WithPath("/exchange-rate/.*"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBody("{}")
                );
            
            foreach (var key in TestData.GetExchangeRatesResponses.Keys)
            {
                WireMock.Server
                    .Given(Request.Create().WithPath($"/exchange-rate/{key}").UsingGet())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithBody(JsonConvert.SerializeObject(TestData.GetExchangeRatesResponses[key]))
                    );
            }
        }

        private async Task RunMain()
        {
            var originalConsoleOut = Console.Out;
            using(var writer = new StringWriter())
            {
                Console.SetOut(writer);

                await Program.Main(Array.Empty<string>());
                
                await Console.Out.FlushAsync();
                await writer.FlushAsync();

                TestData.Lines = writer.GetStringBuilder().ToString().Split(Environment.NewLine);
            }

            Console.SetOut(originalConsoleOut);
        }

        [Then("the program prints out \"(.*)\"")]
        public void ThenTheProgramPrintsOut(string expectedLine)
        {
            TestData.Lines.Should().Contain(expectedLine);
        }

        [Then(@"the program doesn't print out ""(.*)""")]
        public void ThenTheProgramDoesntPrintOut(string unexpectedLine)
        {
            TestData.Lines.Should().NotContain(unexpectedLine);
        }
    }
}