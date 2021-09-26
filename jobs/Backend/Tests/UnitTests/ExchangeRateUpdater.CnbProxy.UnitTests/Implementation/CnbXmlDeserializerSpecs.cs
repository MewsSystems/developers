using System;
using System.IO;
using ExchangeRateUpdater.CnbProxy.Implementation;
using Machine.Specifications;

namespace ExchangeRateUpdater.CnbProxy.UnitTests.Implementation
{
    // TODO: Just one test to show general approach
    
    [Subject(typeof(CnbXmlDeserializer), "Xml deserializer to model generated from xsd schema")]
    class CnbXmlDeserializerSpecs
    {
        private static CnbXmlDeserializer subject;
        private static kurzy result;
        private static string xmlContent;

        Establish context = () =>
        {
            xmlContent = File.ReadAllText(@".\TestData\CnbExchangeRates.xml");

            subject = new CnbXmlDeserializer();
        };

        Because of = () => result = subject.Deserialize<kurzy>(xmlContent);

        It should_not_be_null = () => result.ShouldNotBeNull();

        It should_have_AUD_currency = () => result.tabulka[0].radek[0].kod.ShouldEqual("AUD");

        It should_have_GBP_currency = () => result.tabulka[0].radek[1].kod.ShouldEqual("GBP");

        It should_have_AUD_exchange_rate = () => result.tabulka[0].radek[0].kurz.ShouldEqual("15,719");

        It should_have_GBP_exchange_rate = () => result.tabulka[0].radek[1].kurz.ShouldEqual("29,649");
    }
}
