using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SOAP.Converters;
using SOAP.Models.ExchangeRates;

namespace SOAP.Hubs;

/// <summary>
/// SignalR hub for real-time exchange rate updates.
/// Requires JWT authentication (Consumer or Admin role).
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class ExchangeRatesHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExchangeRatesHub> _logger;

    public ExchangeRatesHub(IMediator mediator, ILogger<ExchangeRatesHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Called when a client connects to the hub.
    /// Sends current exchange rates to the new subscriber.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var userName = Context.User?.Identity?.Name;

        await Clients.Caller.SendAsync("Connected", new
        {
            Message = "Successfully connected to Exchange Rates Hub",
            ConnectionId = Context.ConnectionId,
            UserId = userId,
            UserName = userName
        });

        // Send current rates to the new subscriber
        try
        {
            _logger.LogInformation("Sending current rates to new subscriber: {ConnectionId}", Context.ConnectionId);

            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query);

            if (rates.Any())
            {
                var groupedRates = rates.ToNestedGroupedSoap();
                var xmlData = SerializeToXml(groupedRates);

                await Clients.Caller.SendAsync("HistoricalRatesUpdated", xmlData);
                _logger.LogInformation("Current rates sent to new subscriber: {ConnectionId}", Context.ConnectionId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending current rates to new subscriber: {ConnectionId}", Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects from the hub.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
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
}
