namespace ExchangeRateUpdater
{
    using System.Net;

    public class HttpClient
    {
        private readonly string baseUrl;

        private readonly WebClient client;

        public HttpClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.client = new WebClient();
        }

        public string Get(string query)
        {
            return this.client.DownloadString(this.baseUrl + query);
        }
    }
}