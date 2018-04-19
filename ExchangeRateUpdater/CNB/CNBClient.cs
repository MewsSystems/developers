using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CNB
{
    /// <summary>
    /// API specification: https://www.cnb.cz/cs/faq/kurzy_devizoveho_trhu.html
    /// HTTP Client should be used as singleton, or at least minimilize number of instances 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class CNBClient : IDisposable
    {
        private readonly HttpClient _httpClient;


        /// <summary>
        /// The base API uri
        /// </summary>
        private const string _baseAddress = "http://www.cnb.cz";


        /// <summary>
        /// API endpoint uri
        /// </summary>
        private const string _getTextDataUrl = "/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="CNBClient"/> class.
        /// </summary>
        public CNBClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
        }

        /// <summary>
        /// Gets the data asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">CNB client failed, response null</exception>
        public async Task<Stream> GetDataAsync()
        {
            var response = await _httpClient.GetAsync(_getTextDataUrl);
            if (response == null) throw new NullReferenceException();

            var result = await response.Content.ReadAsStreamAsync();
            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
        }
    }
}
