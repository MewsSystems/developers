require_relative 'exchange_rate_adapter_interface'
require_relative '../../domain/exchange_rate'
require_relative '../../domain/currency'
require_relative '../../errors/exchange_rate_errors'
require_relative '../../services/utils/logging_helper'
require_relative '../../services/utils/format_helper'
require_relative '../utilities/error_handler'
require_relative '../utilities/data_validator'
require_relative '../utilities/domain_factory'

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
      Adapters::Utilities::ErrorHandler.validate_data_presence(data, @provider_name)
      perform_parse(data, Adapters::Utilities::DomainFactory.standardize_currency_code(base_currency_code))
    rescue ExchangeRateErrors::Error => e
      # Re-raise ExchangeRateErrors
      raise e
    rescue ParseError, UnsupportedFormatError => e
      # Convert legacy errors to new error classes
      raise Adapters::Utilities::ErrorHandler.handle_adapter_error(e, @provider_name)
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

  # Helper methods for subclasses
  
  # Ensure UTF-8 encoding
  def ensure_utf8_encoding(data, source_encoding = 'UTF-8')
    begin
      Adapters::Utilities::DomainFactory.ensure_utf8_encoding(data, source_encoding)
    rescue => e
      raise ExchangeRateErrors::ParseError.new(e.message, e, @provider_name)
    end
  end

  # Extract date from string
  def extract_date(date_str)
    Adapters::Utilities::DomainFactory.extract_date(date_str)
  end

  # Standardize currency code
  def standardize_currency_code(code)
    Adapters::Utilities::DomainFactory.standardize_currency_code(code)
  end

  # Parse rate value
  def parse_rate_value(value)
    Utils::FormatHelper.parse_rate_value(value)
  end

  # Create currency object
  def create_currency(code, name = nil)
    Adapters::Utilities::DomainFactory.create_currency(code, name)
  end

  # Create exchange rate object
  def create_exchange_rate(base_currency, target_currency, rate, amount = 1, date = Date.today)
    Adapters::Utilities::DomainFactory.create_exchange_rate(
      base_currency, target_currency, rate, amount, date
    )
  end

  # Validate rate
  def validate_rate(rate_str, currency_code = nil)
    Adapters::Utilities::DataValidator.validate_rate(rate_str, currency_code || 'unknown', @provider_name)
  end

  # Validate amount
  def parse_amount(amount_str, currency_code = nil)
    Adapters::Utilities::DataValidator.validate_amount(amount_str, currency_code || 'unknown', @provider_name)
  end
  
  # Parse rate value from string (for backward compatibility)
  # @param rate_str [String] Rate as string
  # @param currency_code [String] Currency code
  # @return [Float] Parsed rate
  def parse_rate(rate_str, currency_code)
    Adapters::Utilities::DataValidator.validate_rate(rate_str, currency_code, @provider_name)
  end

  # Log an error
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
end