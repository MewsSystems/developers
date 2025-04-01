require_relative '../../errors/exchange_rate_errors'

module Adapters
  module Utilities
    # Centralized error handling for adapters
    class ErrorHandler
      # Handle and standardize errors from adapters
      # @param error [Exception] Original exception
      # @param provider_name [String] Provider name
      # @param context [Hash] Additional context for the error
      # @return [Exception] Standardized error
      def self.handle_adapter_error(error, provider_name, context = {})
        case error
        when ExchangeRateErrors::Error
          # Already a standardized error
          error
        when BaseAdapter::ParseError
          # Legacy parse error
          ExchangeRateErrors::ParseError.new(error.message, error, provider_name, context)
        when BaseAdapter::UnsupportedFormatError
          # Legacy format error
          ExchangeRateErrors::UnsupportedFormatError.new(error.message, error, provider_name, context)
        else
          # Generic error
          ExchangeRateErrors::ParseError.new(
            "Error parsing data: #{error.message}", error, provider_name, context
          )
        end
      end

      # Raise appropriate error for unsupported format
      # @param provider_name [String] Provider name
      # @param format_type [String] Format type description (e.g., "file extension", "content type")
      # @param format_value [String] Actual format value
      # @param context [Hash] Additional context for the error
      # @raise [ExchangeRateErrors::UnsupportedFormatError] Standardized error
      def self.raise_unsupported_format_error(provider_name, format_type, format_value, context = {})
        # Special case for CNB provider with content types for backward compatibility
        if provider_name == 'CNB' && format_type == 'content type'
          raise BaseAdapter::UnsupportedFormatError, "Content type '#{format_value}' not supported"
        end

        context_hash = { format_type.tr(' ', '_') => format_value }
        context_hash.merge!(context) if context.is_a?(Hash)

        raise ExchangeRateErrors::UnsupportedFormatError.new(
          "No adapter found for #{format_type}: #{format_value}",
          nil, provider_name, context_hash
        )
      end

      # Validate that data is provided
      # @param data [String] Data to validate
      # @param provider_name [String] Provider name
      # @raise [ExchangeRateErrors::ValidationError] If data is missing or empty
      def self.validate_data_presence(data, provider_name)
        return if data && !data.to_s.strip.empty?

        raise ExchangeRateErrors::ValidationError.new(
          "Empty data provided",
          nil, provider_name
        )
      end
    end
  end
end