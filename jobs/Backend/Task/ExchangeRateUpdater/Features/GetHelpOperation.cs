using ExchangeRateUpdater.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Features
{
    internal class GetHelpOperation : IOperation
    {
        private readonly IMediator _mediator;

        public GetHelpOperation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ExecuteAsync(IEnumerable<object> args, CancellationToken cancellationToken)
        {
            await _mediator.Send(new Query(), cancellationToken);
        }

        public record Query : IRequest;

        public class RequestHandler : IRequestHandler<Query>
        {
            private readonly string _message = $"USAGE: {AppDomain.CurrentDomain.FriendlyName}\n\n" +
                "--help : Shows this information.\n" +
                "--exchange_rates/-e : Gets the exchange rates.";

            public Task Handle(Query request, CancellationToken cancellationToken)
            {
                Console.WriteLine(_message);
                return Task.CompletedTask;
            }
        }
    }
}
