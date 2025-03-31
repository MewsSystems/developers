require_relative 'base_provider'
require_relative '../adapters/adapter_factory'
require_relative '../fetchers/http_fetcher'
require_relative '../errors/exchange_rate_errors'
require_relative '../services/utils/provider_config'

class ECBProvider < BaseProvider
  # No need for custom initialize method as it just calls super
  # Default configuration will be applied through BaseProvider

  private
  
  # Provider-specific validation for ECB rates
  # @param rates [Array<ExchangeRate>] Exchange rates
  # @raise [ExchangeRateErrors::ValidationError] If rates are invalid
  def validate_provider_specific_rates(rates)
    # Verify all rates are positive (ECB-specific check)
    invalid_rate = rates.find { |rate| rate.rate <= 0 }
    if invalid_rate
      raise ExchangeRateErrors::ValidationError.new(
        "Invalid non-positive rate for #{invalid_rate.to.code}: #{invalid_rate.rate}",
        nil, "ECB", { currency: invalid_rate.to.code, rate: invalid_rate.rate }
      )
    end
  end
end 