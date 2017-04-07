using System;
using System.Net.Http;
using System.Web.Http;

namespace ExchangeRateProvider.Infrastructure.HttpHelper
{
    /// <summary>
    /// Provides an easy mechanism for issuing HTTP requests.
    /// </summary>
    public interface IHttpHelper
    {
        /// <summary>
        /// Issues a request (GET) for retrieving one or more objects.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="address">The address.</param>
        /// <param name="completeAction">An action to invoke when the request completes.</param>
        /// <param name="failAction">An action to invoke when the request fails.</param>
        void Get<TResult>(string address, Action<TResult> completeAction, Action<HttpError> failAction);

        /// <summary>
        /// Issues a request (PUT) for updating an object.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="address">The address.</param>
        /// <param name="getPayload">The get payload.</param>
        /// <param name="completeAction">An action to invoke when the request completes.</param>
        /// <param name="failAction">An action to invoke when the request fails.</param>
        void Put<TResult>(string address, Func<HttpContent> getPayload, Action<TResult> completeAction, Action<HttpError> failAction);

        /// <summary>
        /// Issues a request (POST) for creating an object.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="address">The address.</param>
        /// <param name="getPayload">The get payload.</param>
        /// <param name="completeAction">An action to invoke when the request completes.</param>
        /// <param name="failAction">An action to invoke when the request fails.</param>
        void Post<TResult>(string address, Func<HttpContent> getPayload, Action<TResult> completeAction, Action<HttpError> failAction);

        /// <summary>
        /// Issues a request (DELETE) for deleting an object
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="address">The address.</param>
        /// <param name="completeAction">An action to invoke when the request completes.</param>
        /// <param name="failAction">An action to invoke when the request fails.</param>
        void Delete<TResult>(string address, Action<TResult> completeAction, Action<HttpError> failAction);
    }
}