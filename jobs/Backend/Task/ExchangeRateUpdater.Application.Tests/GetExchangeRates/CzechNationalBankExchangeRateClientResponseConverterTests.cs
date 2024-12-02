using ExchangeRateUpdater.Application.GetExchangeRates;

namespace ExchangeRateUpdater.Application.Tests.GetExchangeRates;

public class CzechNationalBankExchangeRateClientResponseConverterTests
{
    private static CzechNationalBankExchangeRateClientResponseConverter _sut => new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Convert_When_empty_input_Then_exception_is_thrown(string input)
    {
        //Act
        var action = () => _sut.Convert(input);
        
        //Assert
        Assert.Throws<ArgumentException>(action);
    }
    
    [Fact]
    public void Convert_When_null_input_Then_exception_is_thrown()
    {
        //Arrange
        string? input = null;
        
        //Act
        var action = () => _sut.Convert(input);
        
        //Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Convert_When_invalid_line_format_Then_exception_is_thrown()
    {
        //Arrange
        const string input = "HEADER1\nHEADER2\nLine1-with-incorrect-format";
        
        //Action
        var action = () => _sut.Convert(input);
        
        //Assert
        Assert.Throws<InvalidExchangeRateLineFormatException>(action);
    }
    
    [Fact]
    public void Convert_When_invalid_amount_format_Then_exception_is_thrown()
    {
        //Arrange
        const string input = "HEADER1\nHEADER2\nColumn1|Column2|IncorrectAmountValue|Column4|Column5";
        
        //Action
        var action = () => _sut.Convert(input);
        
        //Assert
        Assert.Throws<InvalidAmountFormatException>(action);
    }
    
    [Fact]
    public void Convert_When_invalid_rate_format_Then_exception_is_thrown()
    {
        //Arrange
        const string input = "HEADER1\nHEADER2\nColumn1|Column2|100|Column4|IncorrectRateValue";
        
        //Action
        var action = () => _sut.Convert(input);
        
        //Assert
        Assert.Throws<InvalidRateFormatException>(action);
    }
    
    [Fact]
    public void Convert_When_invalid_source_currency_format_Then_exception_is_thrown()
    {
        //Arrange
        const string input = "HEADER1\nHEADER2\nColumn1|Column2|100|InvalidCurrencyCode|5.99";
        
        //Action
        var action = () => _sut.Convert(input);
        
        //Assert
        Assert.Throws<InvalidCurrencyCodeException>(action);
    }
    
    [Fact]
    public void Convert_When_all_is_valid_Then_result_is_as_expected()
    {
        //Arrange
        const string input = "HEADER1\nHEADER2\nColumn1|Column2|100|EUR|5.999\nColumn1|Column2|2|USD|8.000";
        
        //Action
        var result = _sut.Convert(input);
        
        //Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("EUR", result[0].SourceCurrency.Code);
        Assert.Equal("CZK", result[0].TargetCurrency.Code);
        Assert.Equal(0.05999m, result[0].Value);
        
        Assert.Equal("USD", result[1].SourceCurrency.Code);
        Assert.Equal("CZK", result[1].TargetCurrency.Code);
        Assert.Equal(4m, result[1].Value);
    }
}