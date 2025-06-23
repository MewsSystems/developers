using ExchangeRateUpdater.Application.Common.Extensions;

namespace ExchangeRateUpdater.Application.Tests.Common.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_When_collection_is_null_Then_result_is_true()
    {
        //Arrange
        IEnumerable<int>? collection = null;

        //Act
        var result = collection.IsNullOrEmpty();

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_When_collection_is_empty_Then_result_is_true()
    {
        //Arrange
        var collection = Enumerable.Empty<int>();

        //Act
        var result = collection.IsNullOrEmpty();

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_When_collection_is_not_empty_Then_result_is_true()
    {
        //Arrange
        var collection = Enumerable.Range(0, 10);

        //Act
        var result = collection.IsNullOrEmpty();

        //Assert
        Assert.False(result);
    }
}