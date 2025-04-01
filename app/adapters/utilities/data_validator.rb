require_relative '../../errors/exchange_rate_errors'

module Adapters
  module Utilities
    # Responsible for validation of adapter input data
    class DataValidator
      # Validate rate value
      # @param rate_str [String] Rate as string
      # @param currency_code [String] Currency code for error message
      # @param provider_name [String] Provider name for error
      # @return [Float] Parsed rate value
      # @raise [ExchangeRateErrors::ValidationError] If rate is invalid
      def self.validate_rate(rate_str, currency_code, provider_name)
        rate_str = rate_str.tr(',', '.') # Replace comma with dot for decimal separator
        begin
          rate = Float(rate_str)
          if rate <= 0
            raise ExchangeRateErrors::ValidationError.new(
              "Invalid non-positive rate '#{rate_str}' for currency #{currency_code}",
              nil, provider_name, { currency: currency_code, rate: rate_str }
            )
          end
          rate
        rescue ArgumentError => e
          raise ExchangeRateErrors::ValidationError.new(
            "Invalid rate format '#{rate_str}' for currency #{currency_code}",
            e, provider_name, { currency: currency_code, rate: rate_str }
          )
        end
      end

      # Validate amount value
      # @param amount_str [String] Amount as string
      # @param currency_code [String] Currency code for error message
      # @param provider_name [String] Provider name for error
      # @return [Integer] Parsed amount value
      # @raise [ExchangeRateErrors::ValidationError] If amount is invalid
      def self.validate_amount(amount_str, currency_code, provider_name)
        amount = Integer(amount_str)
        if amount <= 0
          raise ExchangeRateErrors::ValidationError.new(
            "Invalid non-positive amount '#{amount_str}' for currency #{currency_code}",
            nil, provider_name, { currency: currency_code, amount: amount_str }
          )
        end
        amount
      rescue ArgumentError => e
        raise ExchangeRateErrors::ValidationError.new(
          "Invalid amount format '#{amount_str}' for currency #{currency_code}",
          e, provider_name, { currency: currency_code, amount: amount_str }
        )
      end
    end
  end
end