module Utils
  class CurrencyHelper
    # ISO 4217 currency code regex pattern
    CURRENCY_CODE_PATTERN = /^[A-Z]{3}$/

    # Currency validation error class
    class InvalidCurrencyCodeError < StandardError; end

    # Normalize currency code: convert to string, trim whitespace, and upcase
    # @param code [String, Symbol] Currency code to normalize
    # @return [String] Normalized currency code
    # @raise [InvalidCurrencyCodeError] If code is invalid after normalization
    def self.normalize_code(code)
      normalized = code.to_s.strip.upcase
      validate_code(normalized)
      normalized
    end

    # Validate that a currency code is a valid ISO 4217 code
    # @param code [String] Currency code to validate
    # @return [Boolean] True if valid
    # @raise [InvalidCurrencyCodeError] If code is invalid
    def self.validate_code(code)
      unless code =~ CURRENCY_CODE_PATTERN
        raise InvalidCurrencyCodeError, "Invalid currency code: '#{code}'. Must be a 3-letter ISO code."
      end
      true
    end

    # Check if a currency code is valid without raising an exception
    # @param code [String] Currency code to check
    # @return [Boolean] True if valid, false otherwise
    def self.valid_code?(code)
      code =~ CURRENCY_CODE_PATTERN ? true : false
    end

    # Normalize an array of currency codes
    # @param codes [Array<String, Symbol>] Currency codes to normalize
    # @return [Array<String>] Normalized currency codes
    # @raise [InvalidCurrencyCodeError] If any code is invalid after normalization
    def self.normalize_codes(codes)
      codes.map { |code| normalize_code(code) }
    end

    # Extract currency codes from exchange rates
    # @param rates [Array<ExchangeRate>] Exchange rates
    # @return [Array<String>] Sorted array of currency codes
    def self.extract_currency_codes(rates)
      rates.map { |rate| rate.to.code }.sort.freeze
    end
  end
end