require_relative '../../utils/logging_helper'
require_relative '../../utils/currency_helper'
require_relative '../../../errors/exchange_rate_errors'

# Service responsible for currency validation and availability checks
class CurrencyValidatorService
  include Utils::LoggingHelper

  def initialize(provider, provider_name)
    @provider = provider
    @provider_name = provider_name
    @unavailable_currencies = {}
  end

  # Validate that requested currencies are supported
  # @param currencies [Array<String>] Requested currencies
  # @param available_rates [Array<ExchangeRate>] Available rates
  # @raise [ExchangeRateErrors::CurrencyNotSupportedError] If currencies not supported
  def validate_requested_currencies(currencies, available_rates)
    # Ensure all currencies are valid
    begin
      normalized_currencies = Utils::CurrencyHelper.normalize_codes(currencies)
    rescue Utils::CurrencyHelper::InvalidCurrencyCodeError => e
      raise ExchangeRateErrors::ValidationError.new(e.message)
    end

    # Check if provider supports the base currency conversion
    supported_currencies = @provider.supported_currencies

    # If provider doesn't list supported currencies, extract from rates
    if supported_currencies.nil? || supported_currencies.empty?
      supported_currencies = Utils::CurrencyHelper.extract_currency_codes(available_rates)
    end

    # Find unsupported currencies
    unsupported = normalized_currencies.reject do |code|
      supported_currencies.include?(code)
    end

    if unsupported.any?
      raise ExchangeRateErrors::CurrencyNotSupportedError.new(
        "Currencies not supported by #{@provider_name}: #{unsupported.join(', ')}",
        nil, @provider_name,
        {
          unsupported: unsupported,
          supported: supported_currencies
        }
      )
    end
  end

  # Check if any requested currencies are not available from the current provider
  # Logs warnings for unavailable currencies to avoid repeated unnecessary requests
  # @param requested_currencies [Array<String>] Requested currency codes
  # @param available_rates [Array<ExchangeRate>] Available rates
  def check_currency_availability(requested_currencies, available_rates)
    available_codes = available_rates.map { |rate| rate.to.code }.to_set

    # Find currencies that are now unavailable
    unavailable_now = requested_currencies.reject do |code|
      available_codes.include?(code)
    end

    return if unavailable_now.empty?

    # Record time of first request for unavailable currencies
    now = Time.now.to_i
    unavailable_now.each do |code|
      @unavailable_currencies[code] ||= now
      log_unavailable_currency(code, available_codes)
    end
  end

  # Log a warning about an unavailable currency (maintained for test compatibility)
  # @param code [String] Unavailable currency code
  # @param available_codes [Set<String>] Available currency codes
  def log_unavailable_currency(code, available_codes)
    message = "Currency '#{code}' was requested but is not available from #{@provider_name}. " \
              "Available currencies: #{available_codes.to_a.sort.join(', ')}"

    log_warning(message)
  end

  # Normalize a currency code by converting to string, trimming whitespace, and upcasing
  # @param code [String] Currency code to normalize
  # @return [String] Normalized currency code
  def normalize_currency_code(code)
    begin
      Utils::CurrencyHelper.normalize_code(code)
    rescue Utils::CurrencyHelper::InvalidCurrencyCodeError => e
      raise ExchangeRateErrors::ValidationError.new(e.message)
    end
  end

  # Raise a standardized error when no exchange rate is available
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @raise [ExchangeRateErrors::NoExchangeRateAvailableError] No exchange rate available error
  def raise_no_exchange_rate_error(from_currency, to_currency)
    raise ExchangeRateErrors::NoExchangeRateAvailableError.new(
      "No exchange rate available for #{from_currency} to #{to_currency}",
      nil, @provider_name,
      { from: from_currency, to: to_currency }
    )
  end

  # Returns a hash of currencies that have been requested but were unavailable
  # The keys are currency codes, values are the timestamp when first requested
  # @return [Hash] Hash of currency codes to timestamps
  def unavailable_currencies
    @unavailable_currencies.dup.freeze
  end
end 