using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Linq;
using ExchangeRateUpdater.responses;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.tools
{
    public abstract class IToolRequest
    {
        protected HttpMethod Method { get; set; }
        protected string Path { get; set; }
        protected NameValueCollection Query { get; set; }
        protected object Content { get; set; }
        protected string BaseUrlKey { get; set; }

        protected IToolRequest(HttpMethod method, string path, NameValueCollection query, object Content)
        {
            this.Method = method;
            this.Path = path;
            this.Query = query ?? new NameValueCollection();
            this.Content = Content;
        }
        public virtual HttpRequestMessage BuildRequest()
        {
            var url =$"{BaseUrlKey}/{Path}";
            if (Query.HasKeys())
            {
                var parameters = Query.AllKeys
                .Where(key => !string.IsNullOrEmpty(Query[key]))
                .Select(key => $"{key}={Query[key]}");
                url += $"?{String.Join("&", parameters)}";
            }
            var request = new HttpRequestMessage(Method, url);
            if (Content != null && Content is HttpContent httpContent)
            {
                request.Content = httpContent;
            }
            return request;
        }

        public virtual async Task<GenericResponse> Execute()
        {
            var result = new GenericResponse
            {
                IsOKCode = false
            };

            try
            {
                var httpRequest = BuildRequest();

                if (httpRequest == null)
                {
                    result.ReturnCode = "ERROR_TOOL_HTTP_CONFIGURE";
                    result.ReturnMessage = "The HTTP request can't be configured.";
                    return result;
                }
                using var httpclient = new HttpClient();
                var httpResponse = await httpclient.SendAsync(httpRequest);
                result.FullResponseMessage = httpResponse;
                result.OriginalResponse = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    result.IsOKCode = true;
                    result.ReturnCode = "OK";
                    result.Type = MediaTypeNames.Application.Json;
                }
                else
                {
                    result.ReturnCode = "ERROR";
                    result.ReturnMessage = $"Request failed due to {httpResponse.ReasonPhrase}";
                }
            }
            catch (Exception e)
            {
                result.ReturnCode = "ERROR_EXCEPTION_" + e.GetType().ToString();
                result.ReturnMessage = e.Message;
            }
            return result;
        }


    }
}