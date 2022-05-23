using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Client.Cnb.Models;
using ExchangeRate.Client.Cnb.Models.Txt;
using ExchangeRate.Client.Cnb.Models.Xml;
using Framework.BaseHttpClient;
using Framework.Converters.Abstract;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Client.Cnb
{
	public class CnbExchangeRateClient : BaseHttpClient, IExchangeRateClient
	{
		private readonly CnbClientConfiguration _cnbClientConfiguration;
		private readonly ILogger<CnbExchangeRateClient> _logger;
		private readonly IXmlConverter _xmlConverter;

		public CnbExchangeRateClient(HttpClient httpClient, IOptions<CnbClientConfiguration> cnbConfiguration, ILogger<CnbExchangeRateClient> logger, IXmlConverter xmlConverter) : base(httpClient, logger)
		{
			_cnbClientConfiguration = cnbConfiguration.Value;
			_logger = logger;
			_xmlConverter = xmlConverter;
		}

		#region Txt

		public async Task<List<TxtExchangeRate>> GetExchangeRatesTxtAsync()
		{
			var content = await GetAsync(_cnbClientConfiguration.CnbTxtClient?.Url);
			return ParseExchangeRateTxt(content);
		}

		#endregion

		#region Xml

		public async Task<XmlExchangeRate> GetExchangeRatesXmlAsync()
		{
			var content = await GetAsync(_cnbClientConfiguration.CnbXmlClient?.Url);
			return _xmlConverter.ConvertFromXml<XmlExchangeRate>(content);
		}

		#endregion

		#region Private members

		private List<TxtExchangeRate> ParseExchangeRateTxt(string content)
		{
			try
			{
				List<TxtExchangeRate> result = new List<TxtExchangeRate>();
				string[] lines = content.Split('\n');
				for (int i = 2; i < lines.Length; i++)
				{
					string[] line = lines[i].Split('|');
					if (line.Length == 5)
					{
						result.Add(new TxtExchangeRate(line[CnbConstants.TextPosition.TxtPositionCountry], line[CnbConstants.TextPosition.TxtPositionCurrency], int.Parse(line[CnbConstants.TextPosition.TxtPositionAmount]), line[CnbConstants.TextPosition.TxtPositionCode], decimal.Parse(line[CnbConstants.TextPosition.TxtPositionRate], CnbConstants.RateFormat)));
					}
				}

				return result;
			}
			catch (Exception e)
			{
				const string errorMessage = "Content from CNB txt api request has invalid format";
				_logger.LogCritical(e, errorMessage);
				throw new ParsingException(errorMessage);
			}
		}

		#endregion
	}
}
