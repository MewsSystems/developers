module ProviderValidation
  extend ActiveSupport::Concern
  
  # Validate basic aspects of exchange rates that apply to all providers
  # @param rates [Array<ExchangeRate>] Exchange rates to validate
  # @param base_currency [String] Expected base currency
  # @param provider_name [String] Provider name for error messages
  # @raise [ExchangeRateErrors::ValidationError] If rates are invalid
  def validate_rates(rates, base_currency, provider_name)
    validate_rates_not_empty(rates, provider_name)
    validate_base_currency(rates, base_currency, provider_name)

    # Call provider-specific validator method if it exists
    if respond_to?(:validate_provider_specific_rates, true)
      validate_provider_specific_rates(rates)
    end
  end

  private
  
  # Check if rates array is empty
  # @param rates [Array<ExchangeRate>] Exchange rates to validate
  # @param provider_name [String] Provider name for error messages
  # @raise [ExchangeRateErrors::ValidationError] If rates array is empty
  def validate_rates_not_empty(rates, provider_name)
    if rates.empty?
      raise_validation_error("No exchange rates found in #{provider_name} data", provider_name)
    end
  end

  # Verify all rates have the correct base currency
  # @param rates [Array<ExchangeRate>] Exchange rates to validate
  # @param base_currency [String] Expected base currency
  # @param provider_name [String] Provider name for error messages
  # @raise [ExchangeRateErrors::ValidationError] If base currency is incorrect
  def validate_base_currency(rates, base_currency, provider_name)
    incorrect_base = rates.find { |rate| rate.from.code != base_currency }
    if incorrect_base
      raise_validation_error(
        "Unexpected base currency: expected #{base_currency}, got #{incorrect_base.from.code}",
        provider_name,
        { expected: base_currency, actual: incorrect_base.from.code }
      )
    end
  end
  
  # Create a validation error with provider context
  # @param message [String] Error message
  # @param provider_name [String] Provider name
  # @param context [Hash] Additional context for the error
  # @raise [ExchangeRateErrors::ValidationError] Validation error
  def raise_validation_error(message, provider_name, context = {})
    raise ExchangeRateErrors::ValidationError.new(
      message,
      nil, provider_name, context
    )
  end
end 