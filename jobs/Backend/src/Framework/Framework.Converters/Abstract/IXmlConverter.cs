using Framework.Exceptions;

namespace Framework.Converters.Abstract
{
	/// <summary>
	/// XML converter
	/// </summary>
	public interface IXmlConverter
	{
		/// <summary>
		/// Convert string XML T
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="value">xml string</param>
		/// <returns>T</returns>
		/// <exception cref="EmptyResultSetException"></exception>
		/// <exception cref="ParsingException"></exception>
		T ConvertFromXml<T>(string value);
	}
}
