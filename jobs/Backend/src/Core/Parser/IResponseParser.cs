using Core.Models;

namespace Core.Parser
{
    public interface IResponseParser
    {
        /// <summary>
        /// Method to parse the response from the exchange rate client
        /// </summary>
        /// <param name="data">Response data to parse</param>
        /// <returns>List of <see cref="ExchangeRate">ExchangeRate</see></returns>
        IEnumerable<ExchangeRate> ParseResponse(string data);
    }
}
