using System.Xml.Serialization;


namespace CzechNationalBankAPI.Model
{
    [XmlRoot(ElementName = "tabulka")]
    public class TableCNB
    {
        [XmlElement(ElementName = "radek")]
        public List<RowCNB> Row { get; set; }

        [XmlAttribute(AttributeName = "typ")]
        public string Type { get; set; }
    }
}
