using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ExchangeRateProvider.Infrastructure.HttpHelper;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json.Linq;

namespace ExchangeRateProvider.Infrastructure.ApiProxy
{
    /// <summary>
    /// ApiProxy for strongly typed http api client implementation
    /// </summary>
    public abstract partial class ApiProxy
    {
        public static event EventHandler<ApiErrorOccurredEventArgs> ApiErrorOccurred;
        private static string _apiAddress;
        private static readonly Lazy<IHttpHelper> _httpHelper;

        static ApiProxy()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                throw new InvalidOperationException("ServiceLocatorProvider wasn't set prior call of ServiceLocator.Current.GetInstance");
            }
            _httpHelper = new Lazy<IHttpHelper>(ServiceLocator.Current.GetInstance<IHttpHelper>);
        }

        public static IHttpHelper Http
        {
            get { return _httpHelper.Value; }
        }

        /// <summary>
        /// Gets the API address from app.config or defaults.
        /// </summary>
        /// <returns></returns>
        private static string GetApiAddress()
        {
            if (!string.IsNullOrEmpty(_apiAddress)) return _apiAddress;

            var address = ConfigurationManager.AppSettings["WebApiPath"];

            if (string.IsNullOrEmpty(address))
            {
                address = $"http://{"http://www.norges-bank.no/" }/api/Currencies?frequency=D2&language=en&idfilter=none&observationlimit=2&returnsdmx=false";
            }

            return (_apiAddress = address);
        }

        private readonly string _controllerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiProxy" /> class.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        protected ApiProxy(string controllerName)
        {
            _controllerName = controllerName;
        }

        private void RaiseApiErrorOccurredEvent(HttpError error)
        {
            var handler = ApiErrorOccurred;
                handler?.Invoke(this, new ApiErrorOccurredEventArgs(error));
        }

        protected void HandleRequestFailure(HttpError error)
        {
            RaiseApiErrorOccurredEvent(error);
        }

        /// <summary>
        /// Constructs the relative path for the resource from a collection of
        /// path segments.
        /// </summary>
        /// <param name="addressParts">The address parts.</param>
        /// <exception cref="OverflowException">
        /// The array is multidimensional and contains more than
        /// <see cref="System.Int32.MaxValue" /> elements.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="addressParts"/> is <see langword="null"/></exception>
        protected string BuildAddress(params object[] addressParts)
        {
            if (addressParts == null) throw new ArgumentNullException(nameof(addressParts));

            if (addressParts.Length == 0) return _controllerName;

            var allParts = new object[addressParts.Length + 1];
            allParts[0] = _controllerName;

            Array.Copy(addressParts, 0, allParts, 1, addressParts.Length);

            return GetApiAddress() + string.Join("/", allParts);
        }

        /// <summary>
        ///  requests the corresponding service's action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controllerAction"></param>
        /// <param name="completeAction">An action to invoke when the request completes.</param>
        /// <exception cref="OverflowException">
        /// The array is multidimensional and contains more than
        /// <see cref="System.Int32.MaxValue" /> elements.
        /// </exception>
        /// <exception cref="InvalidOperationException">Http is null</exception>
        protected void Get<T>(string controllerAction, Action<T> completeAction)
        {
            if (Http == null) throw new InvalidOperationException("Http is null");
            Http.Get( BuildAddress(controllerAction), completeAction, HandleRequestFailure);
        }
    }
}
