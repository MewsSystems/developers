using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.Data.Repositories;
using ExchangeRateUpdater.Infrastructure.HttpClients;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ExchangeRateUpdaterStepDefinitions
    {
        IEnumerable<ExchangeRate>? _obtainedExchangeRates;
        System.Diagnostics.Process? _process;


        [Given("we have the ExchangeRateUpdater Api running")]
        public void GivenWeHaveTheExchangeRateUpdaterApiRunning()
        {
            LaunchExchangeRateUpdaterApi();
        }

        private void LaunchExchangeRateUpdaterApi()
        {
            _process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/C cd ..\..\..\..\ExchangeRateUpdater.Api\ && dotnet run";
            _process.StartInfo = startInfo;
            _process.Start();
        }

        [When("we call the api\\/exchange-rates endpoint")]
        public void WhenWeCallTheApiExchange_RatesEndpoint()
        {
            using var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $" http://localhost:5129/api/exchange-rates");

            var response = httpClient.SendAsync(httpRequestMessage);
            response.Wait(30000);

            if (response.Result.IsSuccessStatusCode)
            {
                var ratesTask = response.Result.Content.ReadAsStringAsync();
                ratesTask.Wait(30000);

                _obtainedExchangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRate>>(ratesTask.Result);
            }
        }

        [Then("the result should be today's exchange rates")]
        public void ThenTheResultShouldBeTodaysExchangeRates()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.cnb.cz");

            var cnbExchangeRates = (new CnbExchangeRateRepository(new CnbApiClient(httpClient))).GetTodayExchangeRatesAsync();
            cnbExchangeRates.Wait(30000);

            _obtainedExchangeRates.Should().NotBeNull();
            _obtainedExchangeRates.Should().BeEquivalentTo(cnbExchangeRates.Result);

            ProcessHelpers.KillProcessAndChildren(_process!.Id);
        }
    }
}
