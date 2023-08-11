using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExchangeRateProvider
{
    public class ExchangeRateProvider
    {
        private const string connectionStringFile = @"./ConnectionString.txt";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = GetConnectionString();

            SqlParameter[] sqlParameters = currencies.Select(currency => new SqlParameter($"@{currency.Code.ToLower().Trim()}", currency.Code.ToUpper().Trim())).ToArray() ;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @$"SELECT SourceCurrency ,Value FROM master.dbo.ExchangeRateData WHERE SourceCurrency in ({string.Join(", ", currencies.Select(curr => $"@{curr.Code.ToLower().Trim()}").ToArray())})";
                

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(sqlParameters);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exchangeRates.Add(new ExchangeRate(new Currency(reader.GetString(0)), new Currency("CZK"), reader.GetDecimal(1)));
                        }
                    }
                }
            }

            return exchangeRates;
        }
        private string GetConnectionString()
        {
            return File.ReadAllLines(connectionStringFile)[0];

        }
    }
}