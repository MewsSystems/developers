using System.Collections.Specialized;
using Mews.ERP.AppService.Features.Fetch.Builders.Interfaces;
using RestSharp;

namespace Mews.ERP.AppService.Features.Fetch.Builders;

// This is a fair example of why interface segregation and dependency injection is better than just making static classes.
// In the first implementation this was a static class, but in order to make the solution more "unit testable" I decided to create IRequestBuilder<T> and IRestRequestBuilder
// So I can inject it via dependency injection, create unit tests to test THIS functionality and to make it more generic in case in the future we need to communicate via a different way than IRestClient.
public class RestRequestBuilder : IRestRequestBuilder
{
    public RestRequest Build(string path, NameValueCollection parameters)
    {
        var request = new RestRequest(path);

        if (parameters.HasKeys())
        {
            var parameterKeys = parameters
                .AllKeys
                .Where(key => !string.IsNullOrEmpty(parameters[key]));

            foreach (var param in parameterKeys)
            {
                request.AddParameter(param!, parameters[param]);
            }
        }
        
        return request;
    }
}