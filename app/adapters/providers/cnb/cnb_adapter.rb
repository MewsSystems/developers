require_relative '../../adapter_factory'
require_relative '../../../errors/exchange_rate_errors'

# Facade for backward compatibility with legacy code
# This delegates to the appropriate adapter strategy
class CnbAdapter
  # Legacy error class for backward compatibility
  class ParseError < StandardError; end

  # This static method maintains backward compatibility with old code
  # It delegates to the appropriate adapter selected via the factory
  # @param data [String] Raw data to parse
  # @param base_currency_code [String] Base currency code
  # @return [Array<ExchangeRate>] Array of exchange rate objects
  def self.parse(data, base_currency_code)
    begin
      # Use the adapter factory to select the appropriate adapter
      adapter = AdapterFactory.for_content('CNB', data)
      adapter.parse(data, base_currency_code)
    rescue ExchangeRateErrors::Error => e
      # Convert new error format to legacy CnbAdapter::ParseError for backward compatibility
      raise ParseError, e.message
    rescue => e
      # Convert any other errors to ParseError with a message
      raise ParseError, "Error parsing CNB data: #{e.message}"
    end
  end
end