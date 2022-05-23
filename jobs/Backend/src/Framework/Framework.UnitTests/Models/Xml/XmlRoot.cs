using System.Xml.Serialization;

namespace Framework.UnitTests.Models.Xml
{
	[XmlRoot(ElementName = "root")]
	public class XmlRoot
	{
		[XmlElement(ElementName = "item")]
		public List<XmlItem>? Items { get; set; }
	}
}
