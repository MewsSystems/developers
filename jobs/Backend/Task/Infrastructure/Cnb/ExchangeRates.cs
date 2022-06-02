namespace ExchangeRateUpdater.Infrastructure.Cnb;

[SerializableAttribute]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "kurzy")]
public class ExchangeRates
{
    [System.Xml.Serialization.XmlAttribute(AttributeName = "banka")]
    public string Bank { get; set; } = default!;

    [System.Xml.Serialization.XmlAttribute(AttributeName = "datum")]
    public string Date { get; set; } = default!;

    [System.Xml.Serialization.XmlAttribute(AttributeName = "poradi")]
    public string Position { get; set; } = default!;

    [System.Xml.Serialization.XmlElementAttribute(ElementName = "tabulka")]
    public ExchangeRateTable Table { get; set; } = default!;
}

[SerializableAttribute]
[System.Xml.Serialization.XmlTypeAttribute]
public class ExchangeRateTable
{
    [System.Xml.Serialization.XmlAttribute(AttributeName = "typ")]
    public string Type { get; set; } = default!;

    [System.Xml.Serialization.XmlElementAttribute(ElementName = "radek")]
    public ExchangeRateRow[] Rows { get; set; } = default!;
}

[SerializableAttribute]
[System.Xml.Serialization.XmlTypeAttribute]
public class ExchangeRateRow
{
    [System.Xml.Serialization.XmlAttribute(AttributeName = "kod")]
    public string Code { get; set; } = default!;
    
    [System.Xml.Serialization.XmlAttribute(AttributeName = "mnozstvi")]
    public string Quantity { get; set; } = default!;
    
    [System.Xml.Serialization.XmlAttribute(AttributeName = "kurz")]
    public string ExchangeRate { get; set; } = default!;
}