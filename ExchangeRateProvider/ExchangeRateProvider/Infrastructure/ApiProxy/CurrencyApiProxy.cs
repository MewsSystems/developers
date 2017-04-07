using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Batch;
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
        public async Task<List<ExchangeRateDto>> GetExchangeRatesAsync()
        {

            var addr = BuildAddress($"Currencies");
            var client = new HttpClient(new HttpClientHandler() {AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip});
            var response = await client.GetAsync(new Uri(addr, UriKind.Absolute), HttpCompletionOption.ResponseContentRead);

                var rates = JsonConvert.DeserializeObject<List<ExchangeRateEntry>>(await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
                var currencyList = rates?.AsExchangeRateEnumerable().ToList();

            return currencyList;
        }
    }
}
