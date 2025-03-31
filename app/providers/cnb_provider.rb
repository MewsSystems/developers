require_relative '../adapters/adapter_factory'
require_relative '../fetchers/http_fetcher'
require_relative 'base_provider'
require_relative '../errors/exchange_rate_errors'
require_relative '../services/utils/provider_helper'
require_relative '../services/utils/provider_config'

class CNBProvider < BaseProvider
  # No need for custom initialize method as it just calls super
  # Default configuration will be applied through BaseProvider

  # Override fetch_data to maintain original error message format for tests
  def fetch_data
    # Use legacy error message format with provider_error_handler instead of perform_provider_operation
    Utils::ProviderHelper.provider_error_handler("CNB", "fetching data") do
      response = HttpFetcher.fetch(@url, {}, 3, "CNB")
      
      # Add content type from configuration if not detected
      response[:content_type] ||= @content_type
      
      response
    end
  end
  
  private
  
  # Provider-specific validation for CNB rates
  # @param rates [Array<ExchangeRate>] Exchange rates
  # @raise [ExchangeRateErrors::ValidationError] If rates are invalid
  def validate_provider_specific_rates(rates)
    # Add any CNB-specific validation here if needed
    # Currently no CNB-specific validations
  end
end 