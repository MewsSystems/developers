using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ExchangeRateUpdater.Tests;

public class IExchangeRateProviderTests : TestBase
{
    [Fact]
    public async Task BasicTest()
    {
        var rates = await _exchangeRateProvider.GetExchangeRates();
        //result must not be null
        Assert.NotNull(rates);
        //result must be greater than zero
        Assert.True(rates.Count() > 0);
        //assume always that a EUR exchange rate exists and is between 1 & 100
        Assert.True(rates.Count(p => p.TargetCurrency.Code == "EUR" && p.Value > 1 && p.Value < 100) > 0);
    }

    const string schemaUri = "schemas/denni_kurz.xsd";
    const string inputUri = "schemas/denni_kurz.xml";

    [Fact]
    public async Task XmlSchemaTest()
    {
        var xml = await _exchangeRateProvider.GetExchangeRatesXml();
        Assert.NotNull(xml);
        using var stringReader = new StringReader(xml);
        var schema = new XmlSchemaSet();
        schema.Add(string.Empty, schemaUri);
        var doc = XDocument.Load(stringReader);
        doc.Validate(schema, ValidationEventHandler);
    }

    [Fact]
    public void XmlSchemaTestLocal()
    {
        using var xmlReader = XmlReader.Create(inputUri);
        var schema = new XmlSchemaSet();
        schema.Add(string.Empty, schemaUri);
        var doc = XDocument.Load(xmlReader);
        doc.Validate(schema, ValidationEventHandler);
    }

    static void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        if (Enum.TryParse("Error", out XmlSeverityType type))
            if (type == XmlSeverityType.Error)
                Assert.Fail("XSD validation failed");
    }
}