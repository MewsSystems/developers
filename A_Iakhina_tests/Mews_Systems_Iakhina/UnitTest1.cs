using System.Net;
using RestSharp;
using NUnit.Framework;

namespace mews
{
    [TestFixture]
    class Program
    {
        string baseUri = "https://demo.mews.li";
        string countries = "/api/connector/v1/countries/getAll";
        string outlets = "/api/connector/v1/outlets/getAll";
        string spaces = "/api/connector/v1/spaces/getAll";

        [Test]
        public void GetCountriesTest()
        {
            var client = new RestClient(baseUri);
            var request = new RestRequest(countries, Method.POST);
            request.RequestFormat = DataFormat.Json;
            PassTokens(request);
            IRestResponse response = client.Execute(request);
            CheckResponse(response);
        }

        [Test]
        public void GetOutletsTest()
        {
            var client = new RestClient(baseUri);
            var request = new RestRequest(outlets, Method.POST);
            request.RequestFormat = DataFormat.Json;
            PassTokens(request);
            IRestResponse response = client.Execute(request);
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void GetSpacesTest()
        {
            var client = new RestClient(baseUri);
            var request = new RestRequest(spaces, Method.POST);
            request.RequestFormat = DataFormat.Json;
            PassTokens(request);
            IRestResponse response = client.Execute(request);
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        private static void PassTokens(RestRequest request)
        {
            request.AddBody(new
            {
                ClientToken = "E0D439EE522F44368DC78E1BFB03710C-D24FB11DBE31D4621C4817E028D9E1D",
                AccessToken = "C66EF7B239D24632943D115EDE9CB810-EA00F8FD8294692C940F6B5A8F9453D"
            }
                        );
        }
        private static void CheckResponse(IRestResponse response)
        {
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
