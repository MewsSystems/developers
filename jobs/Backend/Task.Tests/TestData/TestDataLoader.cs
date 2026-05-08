using System.IO;
using System.Reflection;

namespace ExchangeRateUpdater.Tests.TestData
{
    // Helper class for loading test data files.
    public static class TestDataLoader
    {
        private static readonly string TestDataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "TestData");

        // Loads a test data file from the TestData directory and return file contents as string
        public static string LoadTestFile(string fileName)
        {
            var path = Path.Combine(TestDataPath, fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Test data file not found: {fileName}", path);
            }

            return File.ReadAllText(path);
        }
    }
}
