namespace Common.Csv
{
    public interface ICsvWrapper
    {
        IEnumerable<T> ParseCsv<T>(string csvData, string delimitter, bool hasHeaderRecord, bool skipFirstRow);
    }
}
