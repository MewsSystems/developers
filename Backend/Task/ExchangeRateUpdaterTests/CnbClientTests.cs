using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdaterTests
{
    public class CnbClientTests
    {
        private const string FullList = "13 Mar 2020 #52\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.728\nBrazil|real|1|BRL|5.006\nBulgaria|lev|1|BGN|13.313\nCanada|dollar|1|CAD|16.920\nChina|renminbi|1|CNY|3.356\nCroatia|kuna|1|HRK|3.445\nDenmark|krone|1|DKK|3.484\nEMU|euro|1|EUR|26.040\nHongkong|dollar|1|HKD|3.019\nHungary|forint|100|HUF|7.684\nIceland|krona|100|ISK|17.360\nIMF|SDR|1|XDR|32.316\nIndia|rupee|100|INR|31.801\nIndonesia|rupiah|1000|IDR|1.588\nIsrael|new shekel|1|ILS|6.370\nJapan|yen|100|JPY|21.865\nMalaysia|ringgit|1|MYR|5.481\nMexico|peso|1|MXN|1.094\nNew Zealand|dollar|1|NZD|14.369\nNorway|krone|1|NOK|2.345\nPhilippines|peso|100|PHP|45.951\nPoland|zloty|1|PLN|5.976\nRomania|leu|1|RON|5.400\nRussia|rouble|100|RUB|32.246\nSingapore|dollar|1|SGD|16.603\nSouth Africa|rand|1|ZAR|1.454\nSouth Korea|won|100|KRW|1.944\nSweden|krona|1|SEK|2.401\nSwitzerland|franc|1|CHF|24.554\nThailand|baht|100|THB|73.948\nTurkey|lira|1|TRY|3.728\nUnited Kingdom|pound|1|GBP|29.230\nUSA|dollar|1|USD|23.449";
        private const string WrongFormatList = "13 Mar 2020 #52\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|14.728\nWRONG_LINE";
        private const string SimpleList = "13 Mar 2020 #52\nCountry|Currency|Amount|Code|Rate\nRussia|rouble|100|RUB|32.246";
        private static HttpResponseMessage FullResponse => new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(FullList)};
        private static HttpResponseMessage WrongFormatResponse => new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(WrongFormatList)};
        private static HttpResponseMessage SimpleResponse => new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(SimpleList)};
        private static HttpResponseMessage FailedResponse => new HttpResponseMessage(HttpStatusCode.Unauthorized) {Content = new StringContent(string.Empty)};
        
        
        private readonly Mock<IHttpClient> _httpClient = new Mock<IHttpClient>();
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CnbClient_FullResponse_Ok()
        {
            _httpClient.Setup(x => x.GetAsync(CnbClient.GetExchangeRatesUrl)).ReturnsAsync(FullResponse);
            var target = new CnbClient(_httpClient.Object);
            var result = await target.GetExchangeRates();
            Assert.AreEqual(33,result.Count());
        }
        
        [Test]
        public void CnbClient_ThrowsException_Handled()
        {
            _httpClient.Setup(x => x.GetAsync(CnbClient.GetExchangeRatesUrl)).Throws(new HttpRequestException());
            var target = new CnbClient(_httpClient.Object);
            Assert.ThrowsAsync<Exception>(async () => await target.GetExchangeRates());
        }

        [Test]
        public void CnbClient_WrongFormat_Handled()
        {
            _httpClient.Setup(x => x.GetAsync(CnbClient.GetExchangeRatesUrl)).ReturnsAsync(WrongFormatResponse);
            var target = new CnbClient(_httpClient.Object);
            Assert.ThrowsAsync<FormatException>(async () =>  await target.GetExchangeRates());
        }
        
        [Test]
        public async Task CnbClient_SimpleList_Ok()
        {
            _httpClient.Setup(x => x.GetAsync(CnbClient.GetExchangeRatesUrl)).ReturnsAsync(SimpleResponse);
            var target = new CnbClient(_httpClient.Object);
            var result = await target.GetExchangeRates();
            Assert.AreEqual(new[]{new ExchangeRate(new Currency("RUB"), new Currency("CZK"), 0.32246m)}, result);
        }
        
        [Test]
        public void CnbClient_ThrowsExceptionOnResponse_Handled()
        {
            _httpClient.Setup(x => x.GetAsync(CnbClient.GetExchangeRatesUrl)).ReturnsAsync(FailedResponse);
            var target = new CnbClient(_httpClient.Object);
            Assert.ThrowsAsync<Exception>(async () => await target.GetExchangeRates());
        }
    }
}