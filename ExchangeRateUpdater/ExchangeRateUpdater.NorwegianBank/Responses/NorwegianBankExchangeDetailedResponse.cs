using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.NorwegianBank.Responses
{
    /// <summary>
    /// Response currency data from Norwegian Bank
    /// </summary>
    /// <remarks>
    /// JSON format:
    /// {
	///     "Updated": "2017-01-13T16:02:02Z",
	///     "TableNameHeader": "Currency (code)",
	///     "TableGraphHeader": "Last mth",
	///     "TableDynamicHeaders": ["13 Jan", "12 Jan"],
	///     "TableEntries": [{
	///     	"Name": "Australian dollar (AUD)",
	///     	"Id": "AUD",
	///     	"ConversionFactor": 1,
	///     	"Values": ["6.3677", "6.3798"],
	///     	"GraphUrl": "/WebDAV/stat/Valutakurser/png/aud.png"
	///     }, {
	///     	"Name": "Bulgarian lev (BGN)",
	///     	"Id": "BGN",
	///     	"ConversionFactor": 100,
	///     	"Values": ["463.14", "463.37"],
	///     	"GraphUrl": "/WebDAV/stat/Valutakurser/png/bgn.png"
	///     }
    /// }
    /// </remarks>
    class NorwegianBankExchangeDetailedResponse
    {
        public IEnumerable<NorwegianBankExchangeRate> TableEntries { get; set; }
        public DateTime Updated { get; set; }
        public string TableNameHeader { get; set; }
        public string TableGraphHeader { get; set; }
    }

    class NorwegianBankExchangeRate : IExhangeRate
    {
        /// <summary>
        /// Code of the currency
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name of the currency
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Relative path to the currency's image
        /// </summary>
        public string GraphUrl { get; set; }
        /// <summary>
        /// Conversion factor of the currency
        /// </summary>
        public int ConversionFactor { get; set; }
        /// <summary>
        /// Array of last currecny rates to NOK
        /// </summary>
        public decimal[] Values { get; set; }

        /// <summary>
        /// Converted exhange rate
        /// </summary>
        public ExchangeRate ExchangeRate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Id))
                    throw new ArgumentException("NorwegianBankExchangeResponse: Code is empty");
                if (ConversionFactor == 0)
                    throw new ArgumentException("NorwegianBankExchangeResponse: ConversionFactor is 0");
                if (Values == null || !Values.Any())
                    throw new ArgumentException("NorwegianBankExchangeResponse: No Values");

                return new ExchangeRate(new Currency(this.Id), new Currency("NOK"), this.Values[0] / ConversionFactor);
            }
        }
    }
}
