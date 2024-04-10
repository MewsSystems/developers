using System.Collections.Specialized;
using System.Web;

namespace ExchangeRates.Http
{
    public class RequestUriBuilder
    {
        private readonly UriBuilder _uriBuilder;

        private readonly NameValueCollection _queryParameters;

        public RequestUriBuilder(string url)
        {
            _uriBuilder = new UriBuilder(url);
            _queryParameters = new NameValueCollection();
        }

        public RequestUriBuilder AddQueryParameter(string parameter, string value)
        {
            _queryParameters.Add(parameter, value);
            return this;
        }

        public string Build()
        {
            if (_queryParameters.Count > 0)
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query.Add(_queryParameters);
                _uriBuilder.Query = query.ToString();
            }

            return _uriBuilder.ToString();
        }
    }
}
