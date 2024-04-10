using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Domain;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Features;

internal class GetExchangeRateOperation : IOperation
{
    private readonly IMediator _mediator;

    public GetExchangeRateOperation(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ExecuteAsync(IEnumerable<object> args, CancellationToken cancellationToken)
    {
        try
        {
            var currencies = args.Select(x => new Currency(x.ToString()));

            var rates = await _mediator.Send(new Query(currencies), cancellationToken);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Could not retrieve exchange rates ({e.Message}).");
        }
    }

    public record Query(IEnumerable<Currency> Currencies) : IRequest<IList<ExchangeRate>>;

    public class RequestHandler : IRequestHandler<Query, IList<ExchangeRate>>
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        private readonly IValidator<Query> _validator;

        public RequestHandler(IExchangeRatesService exchangeRatesService, IValidator<Query> validator)
        {
            _exchangeRatesService = exchangeRatesService;
            _validator = validator;
        }

        public async Task<IList<ExchangeRate>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.ToString());
            }

            var exchangeRates = await _exchangeRatesService.GetExchangeRatesAsync(cancellationToken);

            return exchangeRates.Where(x =>
                request.Currencies.Contains(x.SourceCurrency) &&
                request.Currencies.Contains(x.TargetCurrency))
                .ToList();
        }
    }

    public class RequestValidator : AbstractValidator<Query>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Currencies)
                .NotEmpty()
                .Must(currencies =>
                {
                    var codes = currencies.Select(c => c.Code);

                    return codes.Distinct().Count() == codes.Count();
                })
                .WithMessage("The provided currencies cannot be empty or contain no duplicates.");
        }
    }
}
