using NUnit.Framework;
using MewsQaInterview.Objects;
using MewsQaInterview.Objects.Response;
using System.Linq;
using System.Net;

namespace MewsQaInterview.Tests
{
    class ConfigurationTests
    {
        [Test]
        public void When_GetConfig_Expect_DataReturned()
        {
            /*
             * Create a Request to get all configurations.
             * Verify the data returned.
             */

            var requestJson = new RequestObject();
            var response = CreateRequest.Post<Configuration>("config", requestJson);
            var currencies = response.Enterprise.Currencies;
            Assert.Multiple(() =>
            {
                Assert.IsTrue(!response.Enterprise.Id.Equals(string.Empty),
                    "Enterprise Id cannot be empty");
                Assert.IsTrue(!response.Enterprise.ChainId.Equals(string.Empty),
                    "Chain Id cannot be empty");
                Assert.GreaterOrEqual(currencies.Count, 1,
                    "At least 1 currency should be available");
                Assert.AreEqual(1, currencies.Where(r => r.IsDefault.Equals(true)).ToList().Count,
                    "Default currency should be only 1");
                Assert.IsNotNull(response.Enterprise.DefaultLanguageCode,
                    "Default language code should not be null.");
                Assert.IsNotNull(response.Enterprise.Address.PostalCode,
                    "Postal code should not be null.");
            });

        }

        [Test]
        public void When_GetConfigInvalid_Expect_400()
        {
            /*
             * Create a Post Request on GetConfig, without tokens.
             * Assert the 400 is returned.
             */            
            var response = CreateRequest.Post<UnauthorizedError>("config", null,
                HttpStatusCode.BadRequest);
        }
    }
}
