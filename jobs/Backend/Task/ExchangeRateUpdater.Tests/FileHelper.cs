namespace ExchangeRateUpdater.Tests
{
    public static class FileHelper
    {
        public static string ReadTextFromFile(string fileName)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            return File.ReadAllText(filePath);
        }
    }
}
