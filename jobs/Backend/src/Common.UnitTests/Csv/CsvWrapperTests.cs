using Common.Csv;
using Moq;

namespace Common.UnitTests.Csv
{
    public class CsvWrapperTests
    {
        private readonly Mock<ICsvWrapper> _mockCsvWrapper;

        public CsvWrapperTests()
        {
            _mockCsvWrapper = new Mock<ICsvWrapper>();
        }

        public class Csv : CsvWrapperTests
        {
            // TODO: Add CSV tests
        }
    }
}
