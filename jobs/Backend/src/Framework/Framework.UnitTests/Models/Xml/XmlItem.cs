using System.Xml.Serialization;

namespace Framework.UnitTests.Models.Xml
{
	[XmlRoot(ElementName = "item")]
	public class XmlItem
	{
		[XmlAttribute(AttributeName = "code")]
		public string? Code { get; set; }

		[XmlAttribute(AttributeName = "value")]
		public int Value { get; set; }
	}
}
