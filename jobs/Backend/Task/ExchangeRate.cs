namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}


System.AggregateException
  HResult=0x80131500
  Message=Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: MediatR.IRequestHandler`2[Application.UseCases.ExchangeRates.GetDailyExchangeRateQuery,FluentResults.Result`1[Application.UseCases.ExchangeRates.GetDailyExchangeRateResponse]] Lifetime: Transient ImplementationType: Application.UseCases.ExchangeRates.GetDailyExchangeRateQueryHandler': Unable to resolve service for type 'Microsoft.Extensions.Logging.ILogger' while attempting to activate 'Application.UseCases.ExchangeRates.GetDailyExchangeRateQueryHandler'.)
  Source=Microsoft.Extensions.DependencyInjection
  StackTrace:
   at Microsoft.Extensions.DependencyInjection.ServiceProvider..ctor(ICollection`1 serviceDescriptors, ServiceProviderOptions options)
   at Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(IServiceCollection services, ServiceProviderOptions options)
   at Microsoft.Extensions.Hosting.HostApplicationBuilder.Build()
   at Microsoft.AspNetCore.Builder.WebApplicationBuilder.Build()
   at Program.<Main>$(String[] args) in D:\technicalTests\TechTest-Jordan\jobs\Backend\Task\API\Program.cs:line 24

  This exception was originally thrown at this call stack:
    [External Code]

Inner Exception 1:
InvalidOperationException: Error while validating the service descriptor 'ServiceType: MediatR.IRequestHandler`2[Application.UseCases.ExchangeRates.GetDailyExchangeRateQuery,FluentResults.Result`1[Application.UseCases.ExchangeRates.GetDailyExchangeRateResponse]] Lifetime: Transient ImplementationType: Application.UseCases.ExchangeRates.GetDailyExchangeRateQueryHandler': Unable to resolve service for type 'Microsoft.Extensions.Logging.ILogger' while attempting to activate 'Application.UseCases.ExchangeRates.GetDailyExchangeRateQueryHandler'.

Inner Exception 2:
InvalidOperationException: Unable to resolve service for type 'Microsoft.Extensions.Logging.ILogger' while attempting to activate 'Application.UseCases.ExchangeRates.GetDailyExchangeRateQueryHandler'.
