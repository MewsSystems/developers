require_relative 'exchange_rate_adapter_interface'
require_relative '../../domain/exchange_rate'
require_relative '../../domain/currency'
require_relative '../../errors/exchange_rate_errors'
require_relative '../../services/utils/logging_helper'
require_relative '../../services/utils/format_helper'

class BaseAdapter
  include ExchangeRateAdapterInterface
  include LoggingHelper

  # For backward compatibility
  class ParseError < StandardError; end
  class UnsupportedFormatError < StandardError; end

  def initialize(provider_name)
    @provider_name = provider_name
  end

  # Default implementation always returns false - subclasses should override
  def supports_content_type?(content_type)
    false
  end

  # Default implementation always returns false - subclasses should override
  def supports_content?(content)
    false
  end

  # Parse the data and convert it to domain ExchangeRate objects
  # @param data [String] Raw data from the provider
  # @param base_currency_code [String] The code of the base currency (e.g., 'CZK')
  # @return [Array<ExchangeRate>] Array of exchange rate domain objects
  def parse(data, base_currency_code)
    begin
      validate_data_presence(data)
      perform_parse(data, standardize_currency_code(base_currency_code))
    rescue ExchangeRateErrors::Error => e
      # Re-raise ExchangeRateErrors
      raise e
    rescue ParseError, UnsupportedFormatError => e
      # Convert legacy errors to new error classes
      error_class = e.is_a?(ParseError) ?
        ExchangeRateErrors::ParseError : ExchangeRateErrors::UnsupportedFormatError

      raise error_class.new(e.message, e, @provider_name)
    rescue => e
      # Convert unexpected errors to ParseError
      raise ExchangeRateErrors::ParseError.new(
        "Error parsing data: #{e.message}", e, @provider_name
      )
    end
  end

  # Implement this method in subclasses to do the actual parsing
  # @param data [String] Raw data from the provider
  # @param base_currency_code [String] The code of the base currency
  # @return [Array<ExchangeRate>] Array of exchange rate domain objects
  def perform_parse(data, base_currency_code)
    raise NotImplementedError, "Subclasses must implement perform_parse"
  end

  protected

  # Delegate to utility functions
  def ensure_utf8_encoding(data, source_encoding = 'UTF-8')
    begin
      Utils::FormatHelper.ensure_utf8_encoding(data, source_encoding)
    rescue => e
      raise ExchangeRateErrors::ParseError.new(e.message, e, @provider_name)
    end
  end

  def extract_date(date_str)
    Utils::FormatHelper.extract_date(date_str) || Date.today
  end

  def standardize_currency_code(code)
    Utils::FormatHelper.standardize_currency_code(code)
  end

  def parse_rate_value(value)
    Utils::FormatHelper.parse_rate_value(value)
  end

  def parse_amount(amount_str, currency_code = nil)
    Utils::FormatHelper.parse_amount(amount_str, currency_code)
  end

  # Create a domain Currency object
  # @param code [String] Currency code
  # @param name [String] Optional currency name
  # @return [Currency] Domain currency object
  def create_currency(code, name = nil)
    Currency.new(code, name)
  end

  # Create an exchange rate object from component parts
  # @param base_currency [String] Base currency code
  # @param target_currency [String] Target currency code
  # @param rate [Float] Exchange rate value
  # @param amount [Integer] Amount for normalization (default: 1)
  # @param date [Date] Date of the rate (default: today)
  # @return [ExchangeRate] Exchange rate object
  def create_exchange_rate(base_currency, target_currency, rate, amount = 1, date = Date.today)
    from = Currency.new(base_currency)
    to = Currency.new(target_currency)

    # Normalize the rate based on amount (e.g., if rate is for 100 units, divide by 100)
    normalized_rate = amount.is_a?(Numeric) ? rate / amount.to_f : rate

    ExchangeRate.new(
      from: from,
      to: to,
      rate: normalized_rate,
      date: date.is_a?(Date) ? date : Date.today
    )
  end

  # Validate that the rate value is valid
  # @param rate_str [String] Rate as string
  # @return [Float] Parsed rate value
  # @raise [ParseError] If the rate is not valid
  def validate_rate(rate_str)
    rate = parse_rate_value(rate_str)
    if rate <= 0
      raise ParseError, "Invalid rate value: #{rate_str}. Must be greater than 0."
    end
    rate
  end

  # Ensure data is provided
  # @param data [String] Data to validate
  # @raise [ParseError] If data is missing or empty
  def validate_data_presence(data)
    unless data && !data.to_s.strip.empty?
      raise ExchangeRateErrors::ValidationError.new(
        "Empty data provided to #{self.class.name}",
        nil, @provider_name
      )
    end
  end

  # Log an error
  # @param message [String] Error message
  # @param level [Symbol] Log level
  def log_error(message, level = :error)
    if defined?(Rails)
      case level
      when :warn, :warning
        Rails.logger.warn(message)
      when :info
        Rails.logger.info(message)
      else
        Rails.logger.error(message)
      end
    else
      warn("[#{level.to_s.upcase}] #{message}")
    end
  end

  # Parse rate value from various formats to float
  # @param rate_str [String] Rate as string
  # @param currency_code [String] Currency code for error message
  # @return [Float] Parsed rate value
  def parse_rate(rate_str, currency_code)
    rate_str = rate_str.gsub(',', '.')  # Replace comma with dot for decimal separator
    begin
      rate = Float(rate_str)
      if rate <= 0
        raise ExchangeRateErrors::ValidationError.new(
          "Invalid non-positive rate '#{rate_str}' for currency #{currency_code}",
          nil, @provider_name, { currency: currency_code, rate: rate_str }
        )
      end
      rate
    rescue ArgumentError => e
      raise ExchangeRateErrors::ValidationError.new(
        "Invalid rate format '#{rate_str}' for currency #{currency_code}",
        e, @provider_name, { currency: currency_code, rate: rate_str }
      )
    end
  end

  # Validate that the amount value is valid
  # @param amount_str [String] Amount as string
  # @param currency_code [String] Currency code for error message
  # @return [Integer] Parsed amount value
  def parse_amount(amount_str, currency_code)
    begin
      amount = Integer(amount_str)
      if amount <= 0
        raise ExchangeRateErrors::ValidationError.new(
          "Invalid non-positive amount '#{amount_str}' for currency #{currency_code}",
          nil, @provider_name, { currency: currency_code, amount: amount_str }
        )
      end
      amount
    rescue ArgumentError => e
      raise ExchangeRateErrors::ValidationError.new(
        "Invalid amount format '#{amount_str}' for currency #{currency_code}",
        e, @provider_name, { currency: currency_code, amount: amount_str }
      )
    end
  end
end