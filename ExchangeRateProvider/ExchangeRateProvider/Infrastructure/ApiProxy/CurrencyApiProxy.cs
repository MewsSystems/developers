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
        public Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync()
        {
            var taskCompletionSource =
                new TaskCompletionSource<IEnumerable<ExchangeRateDto>>();

            var addr = BuildAddress(@"Currencies");
            Http.Get<string>(addr, response =>
            {
               var rates = (RootObject) JsonConvert.DeserializeObject(response,
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                        ObjectCreationHandling = ObjectCreationHandling.Auto,
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        NullValueHandling = NullValueHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.All
                    });

                    var currencyList = rates.TableEntries.AsExchangeRateEnumerable().ToList();
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
