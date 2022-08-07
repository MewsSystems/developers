namespace CurrencyExchangeService.Utilities
{
    using System.Xml.Serialization;
    using CurrencyExchangeService.Interfaces;
    using CurrencyExchangeService.Models;
    using Logger;

    public class XmlSerializationHelper : ISerializationHelper<CurrencyRateXmlResponse>
    {
        private readonly ILogger _logger;
        public XmlSerializationHelper(ILogger logger)
        {
            this._logger = logger;
        }

        public CurrencyRateXmlResponse Deserialize(string value)
        {
            var response = new CurrencyRateXmlResponse();
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(CurrencyRateXmlResponse));

                using (StringReader sr = new StringReader(value))
                {
                    response = (CurrencyRateXmlResponse)ser.Deserialize(sr);
                    _logger.Info($"XmlSerializationHelper:Deserialize: Deserialize {response.Currencies.CurrencyItem.Length} currencies");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"XmlSerializationHelper:Deserialize: Deserialization faild with error: {ex.Message}");
            }

            return response;
        }

        public string Serialize(CurrencyRateXmlResponse value)
        {
            // Could be implemented
            throw new NotImplementedException();
        }
    }
}
