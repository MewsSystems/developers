using System.Text.RegularExpressions;

namespace ExchangeRatesSearcherService.Parser;

public static class StringResponseParser
{
    public static IReadOnlyList<string> Parse(string response)
    {
        var lines = response.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var validLines = new List<string>();

        ValidateDateLine();
        ValidateColumnNamesLine();
        ValidateExchangeRateLines();

        return validLines;

        void ValidateDateLine()
        {
            var dateLine = lines[0];
            if (string.IsNullOrEmpty(dateLine))
            {
                throw new Exception("Invalid response message. Date information is missing.");
            }

            var match = Regex.Match(dateLine, "[0-9]{2} .* #[0-9]*");

            if (match.Success == false)
            {
                throw new Exception("Invalid response message. Date information has incorrect format.");
            }

            validLines.Add(dateLine);
        }

        void ValidateColumnNamesLine()
        {
            var columnNamesLine = lines[1];
            if (string.IsNullOrEmpty(columnNamesLine))
            {
                throw new Exception("Invalid response message. Column information is missing.");
            }

            validLines.Add(columnNamesLine);
        }

        void ValidateExchangeRateLines()
        {
            for (var i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line) == false)
                {
                    validLines.Add(line);
                }
            }
        }
    }
}