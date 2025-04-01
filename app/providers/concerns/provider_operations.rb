module ProviderOperations
  extend ActiveSupport::Concern
  
  # Centralized error handling for provider operations
  # @param provider_name [String] Name of the provider
  # @param operation_name [String] Name of the operation being performed
  # @yield Block to execute with error handling
  # @return [Object] Result of the block
  # @raise [ExchangeRateErrors::Error] If an error occurs
  def perform_provider_operation(provider_name, operation_name)
    Utils::ProviderHelper.with_provider_error_handling(provider_name, operation_name) do
      yield
    end
  end
  
  # Get the list of currency codes supported by this provider
  # @param rates [Array<ExchangeRate>] Exchange rates
  # @return [Array<String>] List of supported currency codes
  def extract_supported_currencies(rates)
    Utils::CurrencyHelper.extract_currency_codes(rates)
  end
  
  # Check if a specific currency is supported by this provider
  # For backward compatibility, this method can be called with either:
  # - One argument: the currency code (using the instance @supported_currencies)
  # - Two arguments: supported_currencies list and the currency code
  #
  # @param arg1 [Array<String>, String] Either supported currencies list or currency code
  # @param arg2 [String, nil] Currency code or nil
  # @return [Boolean] Whether the currency is supported
  def supports_currency?(arg1, arg2 = nil)
    if arg2.nil?
      # Called with just the currency code
      code = arg1
      currencies = @supported_currencies || supported_currencies
    else
      # Called with supported currencies and code
      currencies = arg1
      code = arg2
    end
    
    return false unless code
    code = code.to_s.strip.upcase
    currencies.include?(code)
  end
end 