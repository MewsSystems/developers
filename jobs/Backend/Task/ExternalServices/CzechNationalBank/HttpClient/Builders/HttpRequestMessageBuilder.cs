using System;
using System.Net.Http;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Builders
{
    public class HttpRequestMessageBuilder
    {
        readonly HttpRequestMessage _request;

        public HttpRequestMessageBuilder()
        {
            _request = new HttpRequestMessage();
        }

        public HttpRequestMessageBuilder WithMethod(HttpMethod httpMethod)
        {
            _request.Method = httpMethod;

            return this;
        }

        public HttpRequestMessageBuilder WithHeader(string key, string value)
        {
            _request.Headers.Add(key, value);

            return this;
        }

        public HttpRequestMessageBuilder WithUrl(Uri uri)
        {
            _request.RequestUri = uri;

            return this;
        }

        public HttpRequestMessageBuilder WithContent(string content)
        {
            _request.Content = new StringContent(content);

            return this;
        }

        public HttpRequestMessage Build()
        {
            return _request;
        }
    }
}