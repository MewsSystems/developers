require_relative '../../utils/provider_helper'
require_relative '../../cache/default_cache_strategy'
require_relative 'provider_validator'

# Factory for creating RateService instances
class RateServiceFactory
  # Create a new RateService with all dependencies
  # @param provider [ExchangeRateProviderInterface] Exchange rate provider
  # @param repository [ExchangeRateRepository] Repository for caching
  # @param cache_strategy [CacheStrategy, nil] Optional cache strategy
  # @return [RateService] New RateService instance
  def self.create(provider, repository, cache_strategy = nil)
    # Validate provider
    ProviderValidator.validate(provider)
    
    # Get provider name
    provider_name = Utils::ProviderHelper.provider_name_without_suffix(provider)
    
    # Create cache strategy if not provided
    unless cache_strategy
      cache_strategy = DefaultCacheStrategy.new(provider, repository)
    end
    
    # Create specialized services
    rate_fetcher = RateFetcherService.new(provider, repository, cache_strategy, provider_name)
    currency_validator = CurrencyValidatorService.new(provider, provider_name)
    rate_calculator = RateCalculatorService.new(provider)
    
    # Create and return the main service
    RateService.new(
      provider,
      rate_fetcher,
      currency_validator,
      rate_calculator,
      provider_name
    )
  end
end 