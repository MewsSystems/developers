using ExchangeRateUpdater.Providers;
using FluentAssertions;
using System.Reflection;


namespace ExchangeRateUpdater.Tests.CnbExchangeRateProviderTests
{
    public class ParseDailyRatesTests
    {
        private readonly object _provider;
        private readonly MethodInfo _parseMethod;

        public ParseDailyRatesTests()
        {
            var type = typeof(CnbExchangeRateProvider);
            _provider = Activator.CreateInstance(type)!;

            _parseMethod = type.GetMethod("ParseDailyRates", BindingFlags.NonPublic | BindingFlags.Instance)!;
        }

        private object InvokeParse(string raw)
        {
            try
            {
                return _parseMethod.Invoke(_provider, new object[] { raw })!;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        [Fact]
        public void Parse_ValidContent_ReturnsRows()
        {
            // Arrange
            var txt = @"03 Jan 2025 #1
                        Country|Currency|Amount|Code|Rate
                        USA|dollar|1|USD|20.796";

            // Act
            var result = InvokeParse(txt);

            // Assert
            var rows = (System.Collections.Generic.IReadOnlyList<object>)result;
            rows.Should().HaveCount(1);
        }

        [Fact]
        public void Parse_InvalidColumnCount_Throws()
        {
            var txt = @"03 Jan 2025 #1
                        H1|H2|H3
                        Bad|Row|Only3Cols";

            Action act = () => InvokeParse(txt);

            act.Should().Throw<FormatException>();
        }

        [Fact]
        public void Parse_InvalidAmount_Throws()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        USA|dollar|XYZ|USD|20.5";

            Action act = () => InvokeParse(txt);

            act.Should().Throw<FormatException>()
                .WithMessage("*amount*");
        }

        [Fact]
        public void Parse_InvalidRate_Throws()
        {
            var txt = @"03 Jan 2025 #1
                        C|C|C|C|C
                        USA|dollar|1|USD|BAD";

            Action act = () => InvokeParse(txt);

            act.Should().Throw<FormatException>()
                .WithMessage("*rate*");
        }

        [Fact]
        public void Parse_EmptyContent_Throws()
        {
            Action act = () => InvokeParse("");

            act.Should().Throw<FormatException>();
        }
    }
}
