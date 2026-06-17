using System.Collections.Specialized;
using AutoFixture;
using FluentAssertions;
using Mews.ERP.AppService.Features.Fetch.Builders;
using Mews.ERP.AppService.Features.Fetch.Builders.Interfaces;
using Moq.AutoMock;

namespace Mews.ERP.AppService.UnitTests.Features.Fetch.Builders;

public class RestRequestBuilderTests
{
    private readonly AutoMocker autoMocker = new();
    
    private readonly Fixture fixture = new();

    private readonly IRestRequestBuilder sut;

    public RestRequestBuilderTests()
    {
        sut = autoMocker.CreateInstance<RestRequestBuilder>();
    }

    [Fact]
    public void When_Build_Is_Called_Request_Should_Be_Built_Successfully_Without_Parameters()
    {
        // Arrange
        var resource = fixture.Create<string>();
        var parameters = new NameValueCollection();

        // Act
        var request = sut.Build(resource, parameters);

        // Assert
        request.Should().NotBeNull();
        request.Resource.Should().BeEquivalentTo(resource);
        request.Parameters.Count.Should().Be(0);
    }

    [Fact]
    public void When_Build_Is_Called_And_Parameters_AreProvided_Then_Request_Should_Be_Built_With_Parameters()
    {
        // Arrange
        var resource = fixture.Create<string>();
        var parameters = new NameValueCollection
        {
            {"test", "testValue"},
            {"lang", "langValue"}
        };

        // Act
        var request = sut.Build(resource, parameters);

        // Assert
        request.Should().NotBeNull();
        request.Resource.Should().BeEquivalentTo(resource);
        request.Parameters.Should().NotBeNullOrEmpty();
        request.Parameters.Count.Should().Be(parameters.AllKeys.Length);
    }
}