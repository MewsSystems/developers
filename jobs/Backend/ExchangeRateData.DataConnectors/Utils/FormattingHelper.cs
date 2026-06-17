using System.Globalization;

namespace ExchangeRateData.DataConnectors.Utils;

/// <summary>
/// Helping class for formatting
/// </summary>
public  class FormattingHelper
{
    /// <summary>
    /// Parsing users input from string to required format of date. If there is no users input then returns today.
    /// </summary>
    /// <param name="usersDateInput">The date given by user</param>
    /// <param name="formatedDate">Date in required format</param>
    /// <returns>true/false</returns>
    public static bool ParseToDateString(string? usersDateInput, out string formatedDate)
    {
        DateTime onlyDate = DateTime.Now.Date;
        bool success = true;

        if (!string.IsNullOrEmpty(usersDateInput))
        {
            string[] pattern = new string[] { "dd.MM.yyyy", "yyyy-MM-dd" };
            success = DateTime.TryParseExact(usersDateInput, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            onlyDate = date.Date;           
        }

        formatedDate = onlyDate.ToString("d");
        return success;

    }
}
