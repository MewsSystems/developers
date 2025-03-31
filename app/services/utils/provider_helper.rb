require_relative '../../errors/exchange_rate_errors'
require_relative 'currency_helper'

module Utils
  module ProviderHelper
    class << self
      # Extract provider name without 'Provider' suffix
      def provider_name_without_suffix(provider)
        if provider.is_a?(String)
          provider.gsub('Provider', '')
        else
          provider.class.name.gsub('Provider', '')
        end
      end

      # Validate that a currency code is valid
      # @param code [String] Currency code to validate
      # @raise [ExchangeRateErrors::ValidationError] If currency code is invalid
      def validate_currency(code)
        begin
          CurrencyHelper.validate_code(code)
        rescue CurrencyHelper::InvalidCurrencyCodeError => e
          raise ExchangeRateErrors::ValidationError.new(e.message)
        end
      end
      
      # Centralized error handling for provider operations
      # @param provider_name [String] Name of the provider
      # @param operation [String] Name of the operation being performed
      # @param legacy_format [Boolean] Whether to use legacy error message format
      # @yield Block to execute with error handling
      # @return [Object] Result of the block
      # @raise [ExchangeRateErrors::Error] If an error occurs
      def with_provider_error_handling(provider_name, operation, legacy_format = false)
        begin
          yield
        rescue ExchangeRateErrors::ParseError => e
          # Format parse error message based on the provider and operation
          message = if provider_name == "CNB" && operation == "fetching rates"
                      "Failed to parse CNB data: #{e.message}"
                    elsif legacy_format
                      "Failed to parse #{provider_name} data: #{e.message}"
                    else
                      "Error #{operation} from #{provider_name}: #{e.message}"
                    end
                    
          raise ExchangeRateErrors::ParseError.new(
            message,
            e.original_error, provider_name
          )
        rescue ExchangeRateErrors::Error => e
          # Re-raise existing ExchangeRateErrors
          raise e
        rescue => e
          # Map other errors to appropriate ExchangeRateErrors
          error_class = error_class_for_exception(e)
          
          # Format the error message based on provider, operation and format
          message = if legacy_format && provider_name == "CNB" && operation == "fetching data"
                      "Failed to fetch data from CNB: #{e.message}"
                    elsif legacy_format && provider_name == "CNB" && operation == "fetching rates"
                      "Error fetching rates from CNB: #{e.message}"
                    else
                      "Error #{operation} from #{provider_name}: #{e.message}"
                    end
          
          raise error_class.new(
            message,
            e, provider_name
          )
        end
      end
      
      # Map standard exceptions to appropriate ExchangeRateErrors
      # @param exception [Exception] The exception to map
      # @return [Class] The ExchangeRateErrors class to use
      def error_class_for_exception(exception)
        case exception
        when Timeout::Error, Net::ReadTimeout, Net::OpenTimeout
          ExchangeRateErrors::TimeoutError
        when JSON::ParserError, Nokogiri::XML::SyntaxError
          ExchangeRateErrors::ParseError
        when OpenSSL::SSL::SSLError
          ExchangeRateErrors::SecurityError
        when Errno::ECONNREFUSED, Errno::ECONNRESET, SocketError
          ExchangeRateErrors::ProviderUnavailableError
        when Net::HTTPClientException
          handle_http_client_exception(exception)
        else
          ExchangeRateErrors::ProviderError
        end
      end

      # Handle HTTP client exceptions 
      # @param exception [Exception] The HTTP client exception
      # @return [Class] The appropriate ExchangeRateErrors class
      def handle_http_client_exception(exception)
        if exception.message.include?('401') || exception.message.include?('403')
          ExchangeRateErrors::ProviderAuthenticationError
        elsif exception.message.include?('429')
          ExchangeRateErrors::ProviderRateLimitError
        else
          ExchangeRateErrors::ProviderError
        end
      end

      # Legacy method for backward compatibility
      # @deprecated Use with_provider_error_handling instead
      def provider_error_handler(provider_name, operation, &block)
        with_provider_error_handling(provider_name, operation, true, &block)
      end
    end
  end
end 