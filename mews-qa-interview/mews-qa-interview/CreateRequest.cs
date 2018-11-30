using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace MewsQaInterview
{
    public static class CreateRequest
    {
        private static Connection Api;

        static CreateRequest()
        {
            if (!ConfigurationManager.AppSettings["proxy"].Equals(String.Empty))
            {
                Api = new Connection(Connection.ClientType.Proxy);
            }
            else
            {
                Api = new Connection(Connection.ClientType.General);
            }
        }

        /// <summary>
        /// Creates a GET request on the endpoint specified in the parameter.
        /// </summary>
        /// <typeparam name="T">POCO of the response object</typeparam>
        /// <param name="resource">Specific endpoint</param>
        /// <param name="code">HTTPStatusCode value that is expected to be
        /// the response. If not specified, 200 is used.
        /// </param>
        /// <returns>Response Object</returns>
        public static T Get<T>(string resource, 
            HttpStatusCode code = HttpStatusCode.OK) where T : new()
        {
            var request = new RestRequest(Method.GET)
            {
                Resource = ConfigurationManager.AppSettings[resource]
            };
            return Api.Execute<T>(request, code);
        }

        /// <summary>
        /// Creates a POST request on the specified endpoint and returns data
        /// </summary>
        /// <typeparam name="T">POCO of the response object</typeparam>
        /// <param name="resource">Specific endpoint</param>
        /// <param name="json">Object added to the body of the request</param>
        /// <param name="code">HTTPStatusCode value that is expected to be
        /// the response. If not specified, 200 is used.
        /// </param>
        /// <returns>Response Object</returns>
        public static T Post<T>(string resource, object json, 
            HttpStatusCode code = HttpStatusCode.OK) where T : new()
        {
            var request = new RestRequest(Method.POST)
            {
                Resource = ConfigurationManager.AppSettings[resource]
            };
            request.AddJsonBody(json);

            return Api.Execute<T>(request, code);
        }
    }
}
