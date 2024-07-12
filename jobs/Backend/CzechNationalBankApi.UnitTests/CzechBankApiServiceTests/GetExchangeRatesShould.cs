using Microsoft.Extensions.Logging;

namespace CzechNationalBankApi.UnitTests.CzechBankApiServiceTests
{
    public class GetExchangeRatesShould
    {
        private CzechBankApiService _czechBankApiService;

        public GetExchangeRatesShould()
        {
            //Null logger and a mockHttpClient needed
            //_czechBankApiService = new CzechBankApiService();
        }

        [Fact]
        public async Task ReturnExpectedData()
        {

        }

        [Fact]
        public void ThrowExceptionAndNotSwallow()
        {

        }
    }
}