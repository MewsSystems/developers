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

    //Note: XSD validation via https://www.c-sharpcorner.com/article/how-to-validate-xml-using-xsd-in-c-sharp/
    [Fact]
    public async Task XmlSchemaTest()
    {
        var xml = await _exchangeRateProvider.GetExchangeRatesXml();
        Assert.NotNull(xml);
        using (var sr = new StringReader(xml))
        {
            var schema = new XmlSchemaSet();
            schema.Add("", "schemas/denni_kurz.xsd");
            var doc = XDocument.Load(sr);
            doc.Validate(schema, ValidationEventHandler);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var type = XmlSeverityType.Warning;
            if (Enum.TryParse("Error", out type))
                if (type == XmlSeverityType.Error)
                    Assert.Fail("xsd validation failed");
        }
    }

    [Fact]
    public void XmlSchemaTestLocal()
    {
        using (var sr = XmlReader.Create("schemas/denni_kurz.xml"))
        {
            var schema = new XmlSchemaSet();
            schema.Add("", "schemas/denni_kurz.xsd");
            var doc = XDocument.Load(sr);
            doc.Validate(schema, ValidationEventHandler);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var type = XmlSeverityType.Warning;
            if (Enum.TryParse("Error", out type))
                if (type == XmlSeverityType.Error)
                    Assert.Fail("xsd validation failed");
        }
    }
}