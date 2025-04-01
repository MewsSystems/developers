require_relative '../../errors/exchange_rate_errors'

module Adapters
  module Utilities
    # Centralized error handling for adapters
    class ErrorHandler
      # Handle and standardize errors from adapters
      # @param e [Exception] Original exception
      # @param provider_name [String] Provider name
      # @param context [Hash] Additional context for the error
      # @return [Exception] Standardized error
      def self.handle_adapter_error(e, provider_name, context = {})
        if e.is_a?(ExchangeRateErrors::Error)
          # Already a standardized error
          return e
        elsif e.is_a?(BaseAdapter::ParseError)
          # Legacy parse error
          return ExchangeRateErrors::ParseError.new(e.message, e, provider_name, context)
        elsif e.is_a?(BaseAdapter::UnsupportedFormatError)
          # Legacy format error
          return ExchangeRateErrors::UnsupportedFormatError.new(e.message, e, provider_name, context)
        else
          # Generic error
          return ExchangeRateErrors::ParseError.new(
            "Error parsing data: #{e.message}", e, provider_name, context
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
          raise BaseAdapter::UnsupportedFormatError.new("Content type '#{format_value}' not supported")
        end
        
        context_hash = { format_type.gsub(' ', '_') => format_value }
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
        unless data && !data.to_s.strip.empty?
          raise ExchangeRateErrors::ValidationError.new(
            "Empty data provided",
            nil, provider_name
          )
        end
      end
    end
  end
end 