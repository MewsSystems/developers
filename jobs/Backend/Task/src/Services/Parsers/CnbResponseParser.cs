using System.Globalization;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public class CnbResponseParser : ICnbResponseParser
{
    private readonly ILogger<CnbResponseParser> _logger;

    public CnbResponseParser(ILogger<CnbResponseParser> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Dictionary<string, decimal> Parse(string xmlContent, DateTime requestedDate)
    {
        var rates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(xmlContent))
        {
            _logger.LogWarning("Empty XML content received from CNB");
            return rates;
        }

        var doc = new XmlDocument();
        try
        {
            doc.LoadXml(xmlContent);
        }
        catch (XmlException ex)
        {
            _logger.LogError(ex, "Failed to parse XML from CNB");
            throw new CnbApiException("Invalid XML structure from CNB", ex);
        }

        var rootNode = doc.DocumentElement;
        if (rootNode == null || rootNode.Name != CnbConstants.XmlRootElementName)
        {
            _logger.LogError("Invalid XML root element. Expected '{Expected}', got '{Element}'", CnbConstants.XmlRootElementName, rootNode?.Name ?? "null");
            throw new CnbApiException($"Invalid XML structure: root element '{CnbConstants.XmlRootElementName}' not found");
        }

        ValidateResponseDate(rootNode, requestedDate);

        var rows = doc.SelectNodes($"//{CnbConstants.XmlRowElementName}");
        if (rows == null || rows.Count == 0)
        {
            _logger.LogWarning("No '{ElementName}' elements found in XML response", CnbConstants.XmlRowElementName);
            return rates;
        }

        var czechCulture = CultureInfo.GetCultureInfo(CnbConstants.CzechCultureCode);

        foreach (XmlNode row in rows)
        {
            var currencyCode = row.Attributes?[CnbConstants.CodeAttributeName]?.Value;
            var amountString = row.Attributes?[CnbConstants.AmountAttributeName]?.Value;
            var rateString = row.Attributes?[CnbConstants.RateAttributeName]?.Value;

            if (string.IsNullOrWhiteSpace(currencyCode) || string.IsNullOrWhiteSpace(amountString) || string.IsNullOrWhiteSpace(rateString))
            {
                continue;
            }

            if (!decimal.TryParse(amountString, NumberStyles.AllowDecimalPoint, czechCulture, out var amount) ||
                !decimal.TryParse(rateString, NumberStyles.AllowDecimalPoint, czechCulture, out var rate))
            {
                continue;
            }

            if (amount <= 0 || rate <= 0)
            {
                continue;
            }

            var normalizedRate = amount > 0 ? rate / amount : rate;
            if (normalizedRate > 0)
            {
                rates[currencyCode] = normalizedRate;
            }
        }

        if (rates.Count == 0)
        {
            _logger.LogWarning("No valid exchange rates found in XML response");
        }
        else
        {
            _logger.LogDebug("Successfully parsed {Count} exchange rates from CNB XML", rates.Count);
        }

        return rates;
    }

    private void ValidateResponseDate(XmlElement rootNode, DateTime requestedDate)
    {
        var dateAttribute = rootNode.Attributes?[CnbConstants.DateAttributeName]?.Value;
        if (string.IsNullOrWhiteSpace(dateAttribute))
        {
            _logger.LogWarning("Date attribute '{AttributeName}' not found in XML response", CnbConstants.DateAttributeName);
            return;
        }

        if (DateTime.TryParseExact(dateAttribute, CnbConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var responseDate))
        {
            if (responseDate.Date != requestedDate.Date)
            {
                _logger.LogWarning(
                    "Date mismatch: Requested date {RequestedDate}, but CNB returned data for {ResponseDate}",
                    requestedDate.ToString(CnbConstants.DateFormat),
                    responseDate.ToString(CnbConstants.DateFormat));
            }
            else
            {
                _logger.LogDebug("Date validation passed: Response date {ResponseDate} matches requested date", responseDate.ToString(CnbConstants.DateFormat));
            }
        }
        else
        {
            _logger.LogWarning("Could not parse date from XML response: '{DateAttribute}'", dateAttribute);
        }
    }
}
