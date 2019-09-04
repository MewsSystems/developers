using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Consts
    {
        public static string ServiceUrl = "https://cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        public static string XmlElementName = "radek";
        public static string XmlAtrributeCurrency = "radek";
        public static string XmlAtrributeQuantity = "mnozstvi";
        public static string XmlAtrributeCode = "kod";
        public static string XmlAtrributeExchRate = "kurz";
        public static string SourceCurrency = "CZK";
    }
}
