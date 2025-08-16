using ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExchangeRateDemo.Application.Tests.HandlerTests.Base
{
    public abstract class HandlerTest<THandler, TRequest, TResult> : BaseHandlerTest
        where THandler : IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        private Mock<ILogger<THandler>> _loggerMock;

        protected abstract THandler Handler { get; }

        protected Mock<ILogger<THandler>> Logger => _loggerMock ??= new Mock<ILogger<THandler>>();

        protected async Task<TResult> Handle(TRequest request)
        {
            var result = await Handler.Handle(request, CancellationToken.None);
            DefaultAssert(request, result);
            return result;
        }

        protected virtual void DefaultAssert(TRequest request, TResult result)
        {
            Assert.IsNotNull(result);
        }
    }

    public abstract class BaseHandlerTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IExchangeRateProvider> _exchangeProviderMock;

        protected Mock<IMediator> Mediator => _mediatorMock ??= new Mock<IMediator>();

        protected Mock<IExchangeRateProvider> ExchangeRateProvider => _exchangeProviderMock ??= new Mock<IExchangeRateProvider>();
    }
}
