using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateProvider.Model;
using ExchangeRateProvider.Model.Extensions;
using Newtonsoft.Json;

namespace ExchangeRateProvider.Infrastructure.ApiProxy
{
    public abstract partial class ApiProxy
    {
        public static readonly CurrencyApiProxy Currency = new CurrencyApiProxy("api");
    }

    public sealed class CurrencyApiProxy : ApiProxy
    {
        /// <summary>
        /// CurrencyApiProxy
        /// </summary>
        internal CurrencyApiProxy(string apiUrl): base(apiUrl)
        {
        }

        /// <summary>
        /// GetExchangeRatesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync()
        {
            TaskCompletionSource<IEnumerable<ExchangeRateDto>> taskCompletionSource = null;
            taskCompletionSource = new TaskCompletionSource<IEnumerable<ExchangeRateDto>>(TaskCreationOptions.LongRunning);

            Http.Get<string>( BuildAddress("Currency"),
                response =>
                {
                    var rates = JsonConvert.DeserializeObject(response) as RootObject;
                    var currencyList = rates?.TableEntries?.AsExchangeRateEnumerable();
                    taskCompletionSource.TrySetResult( currencyList );
                },
                err =>
                {
                    taskCompletionSource.TrySetCanceled();
                    HandleRequestFailure(err);
                });

            return await taskCompletionSource.Task.ConfigureAwait(false);
        }
    }
}
