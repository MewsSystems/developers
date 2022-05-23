namespace Framework.UnitTests
{
	public static class TestFrameworkConstants
	{
		public const string XmlStringOkResponse = @"<?xml version=""1.0"" encoding=""UTF-8""?><root><item code=""EUR"" value=""1""/><item code=""USD"" value=""2""/></root>";

		public const string XmlStringWrongResponse = @"<?xml version=""1.0"" encoding=""UTF-8""?><root><itemxx </root>";
	}
}
