using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SOAP.Converters;
using SOAP.Hubs;
using SOAP.Models.ExchangeRates;

namespace SOAP.Services;

/// <summary>
/// Service that sends real-time exchange rate notifications via SignalR.
/// Sends data as XML strings to be consistent with SOAP protocol.
/// </summary>
public class SignalRExchangeRatesNotificationService : IExchangeRatesNotificationService
{
    private readonly IHubContext<ExchangeRatesHub> _hubContext;
    private readonly IMediator _mediator;
    private readonly ILogger<SignalRExchangeRatesNotificationService> _logger;

    public SignalRExchangeRatesNotificationService(
        IHubContext<ExchangeRatesHub> hubContext,
        IMediator mediator,
        ILogger<SignalRExchangeRatesNotificationService> logger)
    {
        _hubContext = hubContext;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Serializes SOAP models to XML string wrapped in SOAP envelope using DataContractSerializer.
    /// </summary>
    private static string SerializeToXml(LatestExchangeRatesGroupedSoap[] data)
    {
        var serializer = new DataContractSerializer(typeof(LatestExchangeRatesGroupedSoap[]));
        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = false,
            Encoding = Encoding.UTF8
        });

        // Write SOAP Envelope
        xmlWriter.WriteStartElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
        xmlWriter.WriteStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");

        // Serialize the data inside the Body
        serializer.WriteObject(xmlWriter, data);

        // Close Body and Envelope
        xmlWriter.WriteEndElement(); // </soap:Body>
        xmlWriter.WriteEndElement(); // </soap:Envelope>

        xmlWriter.Flush();
        return stringWriter.ToString();
    }

    public async Task NotifyHistoricalRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending historical rates update notification to SignalR clients");

            // Query all latest exchange rates
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query, cancellationToken);

            // Convert to grouped SOAP format
            var groupedRates = rates.ToNestedGroupedSoap();

            // Serialize to XML string
            var xmlData = SerializeToXml(groupedRates);

            // Send XML string to all connected clients
            await _hubContext.Clients.All.SendAsync(
                "HistoricalRatesUpdated",
                xmlData,
                cancellationToken
            );

            _logger.LogInformation("Historical rates notification sent successfully to all clients as XML");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending historical rates notification");
        }
    }

    public async Task NotifyLatestRatesUpdatedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending latest rates update notification to SignalR clients");

            // Query all latest exchange rates
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query, cancellationToken);

            // Convert to grouped SOAP format
            var groupedRates = rates.ToNestedGroupedSoap();

            // Serialize to XML string
            var xmlData = SerializeToXml(groupedRates);

            // Send XML string to all connected clients
            await _hubContext.Clients.All.SendAsync(
                "LatestRatesUpdated",
                xmlData,
                cancellationToken
            );

            _logger.LogInformation("Latest rates notification sent successfully to all clients as XML");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending latest rates notification");
        }
    }
}
