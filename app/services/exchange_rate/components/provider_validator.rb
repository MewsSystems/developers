require_relative '../../../errors/exchange_rate_errors'

# Class responsible for validating exchange rate providers
class ProviderValidator
  # Validate that a provider implements the required interface
  # @param provider [Object] The provider to validate
  # @raise [ExchangeRateErrors::InvalidConfigurationError] If provider is invalid
  def self.validate(provider)
    unless provider.respond_to?(:fetch_rates) && provider.respond_to?(:metadata)
      raise ExchangeRateErrors::InvalidConfigurationError.new(
        "Provider must implement ExchangeRateProviderInterface",
        nil, nil, { provider_class: provider.class.name }
      )
    end
    
    provider
  end
end 