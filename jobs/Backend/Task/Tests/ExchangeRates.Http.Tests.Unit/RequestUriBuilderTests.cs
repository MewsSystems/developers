using FluentAssertions;

namespace ExchangeRates.Http.Tests.Unit
{
    public class RequestUriBuilderTests
    {
        [Fact]
        public void RequestUriBuilder_ThrowsUriFormatException_WithInvalidPath()
        {
            // Arrange.
            var url = "https://local)host:1234/";

            // Act.
            Action action = () => new RequestUriBuilder(url);

            // Assert.
            action.Should().Throw<UriFormatException>();
        }

        [Fact]
        public void Build_ReturnsSamePath_WithValidPathAndWithoutQueryParameters()
        {
            // Arrange.
            var url = "https://localhost:1234/";
            var sut = new RequestUriBuilder(url);

            // Act.
            var res = sut.Build();

            // Assert.
            res.Should().Be(res);
        }

        [Fact]
        public void Build_ReturnsUrl_WithQueryParameters()
        {
            // Arrange.
            var url = "https://localhost:1234/";
            var sut = new RequestUriBuilder(url)
                .AddQueryParameter("key", "value");

            // Act.
            var res = sut.Build();

            // Assert.
            res.Should().Be(url + "?key=value");
        }

        [Fact]
        public void Build_ReturnsEncodedUrl_WithNotUrlEncodedQueryParameters()
        {
            // Arrange.
            var url = "https://localhost:1234/";
            var sut = new RequestUriBuilder(url)
                .AddQueryParameter("key", "some value");

            // Act.
            var res = sut.Build();

            // Assert.
            res.Should().Be(url + "?key=some+value");
        }
    }
}
