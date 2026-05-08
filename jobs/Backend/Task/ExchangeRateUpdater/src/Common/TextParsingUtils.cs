namespace ExchangeRateUpdater.Common
{
    /// <summary>
    /// Provides static utility methods for parsing text-based data.
    /// </summary>
    public static class TextParsingUtils
    {
        /// <summary>
        /// Analyzes a sample of data lines to automatically detect the most likely column delimiter.
        /// </summary>
        /// <param name="lines">The collection of all lines from the source file.</param>
        /// <returns>The detected delimiter character, or ',' as a default fallback.</returns>
        public static char DetectDelimiter(IEnumerable<string> lines)
        {
            var potentialDelimiters = new[] { '|', ',', ';', '\t' };
            var sampleLines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).Take(10).ToList();

            if (sampleLines.Count < 2)
            {
                return ',';
            }

            var delimiterInfo = potentialDelimiters
                .Select(d => new
                {
                    Delimiter = d,
                    MostCommonGroup = sampleLines.GroupBy(l => l.Count(c => c == d))
                                                 .Where(g => g.Key > 0) 
                                                 .OrderByDescending(g => g.Count()) 
                                                 .ThenByDescending(g => g.Key) 
                                                 .Select(g => new { DelimiterCount = g.Key, LineCount = g.Count() })
                                                 .FirstOrDefault()
                })
                .Where(info => info.MostCommonGroup != null && info.MostCommonGroup.LineCount > 1)
                .OrderByDescending(info => info.MostCommonGroup?.LineCount)
                .ThenByDescending(info => info.MostCommonGroup?.DelimiterCount)
                .FirstOrDefault();
            
            return delimiterInfo?.Delimiter ?? ',';
        }

        /// <summary>
        /// Finds the zero-based index of the header row in the data file.
        /// </summary>
        /// <param name="lines">The collection of all lines from the source file.</param>
        /// <param name="delimiter">The delimiter character to look for in the header.</param>
        /// <returns>The index of the header row, or -1 if not found.</returns>
        public static int FindHeaderRowIndex(IReadOnlyList<string> lines, char delimiter)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains(delimiter))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
