require_relative '../../domain/exchange_rate'
require_relative '../../domain/currency'
require_relative '../../services/utils/format_helper'

module Adapters
  module Utilities
    # Factory for creating domain objects
    class DomainFactory
      # Create a currency object
      # @param code [String] Currency code
      # @param name [String] Optional currency name
      # @return [Currency] Domain currency object
      def self.create_currency(code, name = nil)
        Currency.new(code, name)
      end
      
      # Create an exchange rate object
      # @param base_currency [String] Base currency code
      # @param target_currency [String] Target currency code
      # @param rate [Float] Exchange rate value
      # @param amount [Integer] Amount for normalization (default: 1)
      # @param date [Date] Date of the rate (default: today)
      # @return [ExchangeRate] Exchange rate object
      def self.create_exchange_rate(base_currency, target_currency, rate, amount = 1, date = Date.today)
        from = Currency.new(base_currency)
        to = Currency.new(target_currency)
        
        # Normalize the rate based on amount
        normalized_rate = amount.is_a?(Numeric) ? rate / amount.to_f : rate
        
        ExchangeRate.new(
          from: from,
          to: to,
          rate: normalized_rate,
          date: date.is_a?(Date) ? date : Date.today
        )
      end
      
      # Helper for standardizing currency codes
      # @param code [String] Currency code to standardize
      # @return [String] Standardized currency code
      def self.standardize_currency_code(code)
        Utils::FormatHelper.standardize_currency_code(code)
      end
      
      # Helper for extracting dates from strings
      # @param date_str [String] Date string to parse
      # @return [Date] Parsed date or today's date if parsing fails
      def self.extract_date(date_str)
        Utils::FormatHelper.extract_date(date_str) || Date.today
      end
      
      # Helper for ensuring UTF-8 encoding
      # @param data [String] Data to encode
      # @param source_encoding [String] Source encoding (default: 'UTF-8')
      # @return [String] UTF-8 encoded data
      def self.ensure_utf8_encoding(data, source_encoding = 'UTF-8')
        Utils::FormatHelper.ensure_utf8_encoding(data, source_encoding)
      end
    end
  end
end 