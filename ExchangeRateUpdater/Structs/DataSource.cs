using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public enum DataSourceEnum {
        CzechNationalBank,
        EuropeCentralBank,
        CustomsAdministrationXML,
        CustomsAdministrationTXT

    }


    public struct KeyFileProperties
    {
        public string WebSouce { get; private set; }
        public string NameCode { get; private set; }
        public string NameRate { get; private set; }
        public string NameAmout { get; private set; }
        public char Separator { get; private set; }
        public char Delimiter { get; private set; }

        /// <summary>
        /// Object unites all important parameters of files
        /// </summary>
        /// <param name="webSource">Link to download</param>
        /// <param name="nameCode">Name code for parsing in file</param>
        /// <param name="nameRate">Name rate for parsing in file</param>
        /// <param name="nameAmout">Name amount for parsing in file. This parameter is optional. But we need in some countries.</param>
        /// <param name="separator">Separator in this dataset. This parameter is optional. Depend on data source file type.</param>
        /// <param name="delimiter">Delimiter in this dataset</param>
        public KeyFileProperties(string webSource, string nameCode, string nameRate, string nameAmount, char separator,char delimiter)
        {
            WebSouce = webSource;
            NameCode = nameCode;
            NameRate = nameRate;
            NameAmout = nameAmount;
            Separator = separator;
            Delimiter = delimiter;
        }

    }

    public class DataSourceRefeces
    {
        public static Dictionary<DataSourceEnum, KeyFileProperties> Codes = new Dictionary<DataSourceEnum, KeyFileProperties>()
        {
            {DataSourceEnum.CzechNationalBank, new KeyFileProperties("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt","kód","kurz","množství",'|',',') },
            {DataSourceEnum.CustomsAdministrationTXT, new KeyFileProperties("www.celnisprava.cz/cz/aplikace/Stranky/kurzy.aspx?er=1&type=TXT","Kód","Kurz","Množství",';',',') },
            {DataSourceEnum.CustomsAdministrationXML, new KeyFileProperties("www.celnisprava.cz/cz/aplikace/Stranky/kurzy.aspx?er=1&type=XML","MENA","KURZ","POCET",'\0',',') },
            {DataSourceEnum.EuropeCentralBank, new KeyFileProperties("http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml","currency","rate",null,'\0','.') }
        };
    }
}
