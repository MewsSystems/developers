using Framework.Converters;
using Framework.Exceptions;
using Framework.UnitTests.Models.Xml;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Framework.UnitTests.Converters
{
	public class ConvertersTests
	{
		[Fact]
		public void ConvertFromXml_OkConversion()
		{
			var mockLogger = new Mock<ILogger<XmlConverter>>();

			var xmlConverter = new XmlConverter(mockLogger.Object);

			var result = xmlConverter.ConvertFromXml<XmlRoot>(TestFrameworkConstants.XmlStringOkResponse);

			Assert.NotNull(result);
			Assert.True(result.Items?.Count == 2);

			var item = result.Items?[0];
			Assert.NotNull(item);
			Assert.Equal("EUR", item?.Code);
			Assert.Equal(1, item?.Value);

			item = result.Items?[1];
			Assert.NotNull(item);
			Assert.Equal("USD", item?.Code);
			Assert.Equal(2, item?.Value);
		}

		[Fact]
		public void ConvertFromXml_EmptyInput()
		{
			var mockLogger = new Mock<ILogger<XmlConverter>>();

			var xmlConverter = new XmlConverter(mockLogger.Object);

			var exception = Assert.Throws<EmptyResultSetException>(() => xmlConverter.ConvertFromXml<XmlRoot>(""));

			Assert.Equal("Cannot convert empty value.", exception.Message);
		}


		[Fact]
		public void ConvertFromXml_XmlParsingException()
		{
			var mockLogger = new Mock<ILogger<XmlConverter>>();

			var xmlConverter = new XmlConverter(mockLogger.Object);

			var exception = Assert.Throws<ParsingException>(() => xmlConverter.ConvertFromXml<XmlRoot>(TestFrameworkConstants.XmlStringWrongResponse));
			Assert.Equal("Content from xml request has invalid format", exception.Message);
		}
	}
}
