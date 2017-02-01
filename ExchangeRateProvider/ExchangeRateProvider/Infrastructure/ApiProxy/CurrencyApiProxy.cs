using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateProvider.Model;
using ExchangeRateProvider.Model.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateProvider.Infrastructure.ApiProxy
{
    public abstract partial class ApiProxy
    {
        public static readonly CurrencyApiProxy Currency = new CurrencyApiProxy("api");
    }

    public sealed class CurrencyApiProxy : ApiProxy
    {
        public CurrencyApiProxy() : base("api")
        {
        }

        /// <summary>
        ///     CurrencyApiProxy
        /// </summary>
        public CurrencyApiProxy(string apiUrl) : base(apiUrl)
        {
        }

        /// <summary>
        ///     GetExchangeRatesAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException">
        /// The array is multidimensional and contains more than
        /// <see cref="System.Int32.MaxValue" /> elements.
        /// </exception>
        public Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync()
        {
            var taskCompletionSource =
                new TaskCompletionSource<IEnumerable<ExchangeRateDto>>();

            var addr = BuildAddress($"Currencies");
            Http.Get<string>(addr, response =>
            {
               var rates =  JsonConvert.DeserializeObject<IEnumerable<ExchangeRateEntry>>(response,
                    new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml | StringEscapeHandling.EscapeNonAscii,
                        ObjectCreationHandling = ObjectCreationHandling.Auto,
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        NullValueHandling = NullValueHandling.Include,
                        TypeNameHandling = TypeNameHandling.All
                    });

                    var currencyList = rates.AsExchangeRateEnumerable().ToList();
                    taskCompletionSource.TrySetResult(currencyList);
                return;
                }, err =>
                {
                    taskCompletionSource.TrySetException(new HttpRequestException(err.ExceptionMessage));
                    HandleRequestFailure(err);
                });

            return taskCompletionSource.Task;
        }
    }
}
