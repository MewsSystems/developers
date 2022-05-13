using Core.Extensions;
using Core.Infra.Dtos;
using CSharpFunctionalExtensions;

namespace Core.Infra.Mappers
{
    public class ExchangeRateDtoMapper : IExchangeRateDtoMapper
    {
        public Result<IReadOnlyCollection<ExchangeRateDto>> Map(string exchangeSource)
        {
            if (string.IsNullOrWhiteSpace(exchangeSource))
            {
                return Result.Success<IReadOnlyCollection<ExchangeRateDto>>(Array.Empty<ExchangeRateDto>());
            }

            string[] lines = exchangeSource.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

            if (lines.Length < 3)
            {
                return Result.Success<IReadOnlyCollection<ExchangeRateDto>>(Array.Empty<ExchangeRateDto>());
            }

            List<ExchangeRateDto> splittedLines = lines
                .Skip(2)
                .Select(l => l.Split('|'))
                .Where(sl => sl != null && sl.Length == 5)
                .Select(sl => new ExchangeRateDto(sl[0], sl[1], sl[2], sl[3], sl[4]))
                .ToList();

            return Result.Success(splittedLines.ToReadOnlyCollection());
        }
    }
}
