namespace ExchangeRateUpdater.Persistence
{
    public interface IFileProvider
    {
        bool TryGetFileContent(out string content);

        bool SaveFile(string content);
    }
}
