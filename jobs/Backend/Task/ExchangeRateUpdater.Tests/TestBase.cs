using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExchangeRateUpdater.Tests
{
    public class TestBase
    {
        protected MockDomain Mock { get; } = new MockDomain();

        protected static bool CompareTwoCollectionsDeeply<TObject>(
            IEnumerable<TObject> expectedCollection,
            IEnumerable<TObject> resultCollection,
            IComparer<TObject> comparer)
        {
            var serializedOrderedExpectedCollection = JsonSerializer.Serialize(expectedCollection.OrderBy(i => i, comparer));
            var serializedOrderedResultCollection = JsonSerializer.Serialize(resultCollection.OrderBy(i => i, comparer));

            return serializedOrderedExpectedCollection.Equals(serializedOrderedResultCollection);
        }
        
        protected static Stream CreateStreamFromString(string str)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(str);
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

