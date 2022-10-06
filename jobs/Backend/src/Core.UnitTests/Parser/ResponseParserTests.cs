using Core.Parser;
using Moq;

namespace Core.UnitTests.Parser
{
    public class ResponseParserTests
    {
        private readonly Mock<IResponseParser> _mockResponseParser;

        public ResponseParserTests()
        {
            _mockResponseParser = new Mock<IResponseParser>();
        }

        public class ResponseParser : ResponseParserTests
        {
            // TODO: Add ResponseParserTests tests
        }
    }
}
