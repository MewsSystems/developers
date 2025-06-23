using System.Text.Json;
using ExchangeRateUpdater.Infrastructure.Cache;
using Moq;
using StackExchange.Redis;

namespace ExchangeRateUpdater.Infrastructure.Tests.Cache;

public class RedisClientTests
{
    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexerMock = new();
    
    private RedisClient _sut => new(_connectionMultiplexerMock.Object);

    [Theory]
    [InlineData("callbackResult")]
    [InlineData((string?)null)]
    public async Task GetAsync_when_type_is_string_and_key_is_empty_Then_callback_result_is_stored_and_returned(string callbackResult)
    {
        //Arrange
        const string key = "key";
        var callback = () => Task.FromResult(callbackResult);
        var expiration = TimeSpan.FromHours(1);

        var databaseMock = new Mock<IDatabase>();
        _connectionMultiplexerMock.Setup(c => c.GetDatabase(-1, null)).Returns(databaseMock.Object);
        
        //Act
        var result = await _sut.GetAsync(key, callback, expiration);
        
        //Assert
        Assert.Equal(callbackResult, result);
        databaseMock.Verify(d => d.StringGetAsync(key, CommandFlags.None), Times.Once);
        databaseMock.Verify(d => d.StringSetAsync(key, callbackResult, expiration, false, When.Always, CommandFlags.None), Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(CallbackResultModels))]
    public async Task GetAsync_when_type_is_class_and_key_is_empty_Then_callback_result_is_stored_and_returned(RestClientTestModel? model)
    {
        //Arrange
        const string key = "key";
        var callback = () => Task.FromResult(model);
        var expiration = TimeSpan.FromHours(1);

        var databaseMock = new Mock<IDatabase>();
        _connectionMultiplexerMock.Setup(c => c.GetDatabase(-1, null)).Returns(databaseMock.Object);
        
        //Act
        var result = await _sut.GetAsync(key, callback, expiration);
        
        //Assert
        Assert.Equal(model, result);
        databaseMock.Verify(d => d.StringGetAsync(key, CommandFlags.None), Times.Once);
        var serializedModel = JsonSerializer.Serialize(model);
        databaseMock.Verify(d => d.StringSetAsync(key, serializedModel, expiration, false, When.Always, CommandFlags.None), Times.Once);
    }
    
    public static TheoryData<RestClientTestModel?> CallbackResultModels = new()
    {
        new RestClientTestModel { Id = 33, Name = "TestModel", Date = DateTime.UtcNow },
        null
    };
    
    public readonly record struct RestClientTestModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public DateTime Date { get; init; }
    }
    
    [Fact]
    public async Task GetAsync_when_type_is_string_and_key_contains_value_Then_callback_result_is_stored_and_returned()
    {
        //Arrange
        const string key = "key";
        var expiration = TimeSpan.FromHours(1);

        var databaseMock = new Mock<IDatabase>();
        _connectionMultiplexerMock.Setup(c => c.GetDatabase(-1, null)).Returns(databaseMock.Object);
        
        const string resultFromCache = "resultFromCache";
        databaseMock.Setup(d => d.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(resultFromCache);
        
        //Act
        var result = await _sut.GetAsync(key, It.IsAny<Func<Task<string>>>(), expiration);
        
        //Assert
        Assert.Equal(resultFromCache, result);
        databaseMock.Verify(d => d.StringSetAsync(key, It.IsAny<RedisValue>(), expiration, false, When.Always, CommandFlags.None), Times.Never);
    }
    
    [Fact]
    public async Task GetAsync_when_type_is_model_and_key_contains_value_Then_callback_result_is_stored_and_returned()
    {
        //Arrange
        const string key = "key";
        var expiration = TimeSpan.FromHours(1);

        var databaseMock = new Mock<IDatabase>();
        _connectionMultiplexerMock.Setup(c => c.GetDatabase(-1, null)).Returns(databaseMock.Object);

        var resultFromCache = new RestClientTestModel { Id = 33, Name = "TestModel", Date = DateTime.UtcNow };
        var serializedResultFromCache = JsonSerializer.Serialize(resultFromCache);
        databaseMock.Setup(d => d.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(serializedResultFromCache);
        
        //Act
        var result = await _sut.GetAsync(key, It.IsAny<Func<Task<RestClientTestModel>>>(), expiration);
        
        //Assert
        Assert.Equal(resultFromCache, result);
        databaseMock.Verify(d => d.StringSetAsync(key, It.IsAny<RedisValue>(), expiration, false, When.Always, CommandFlags.None), Times.Never);
    }
}