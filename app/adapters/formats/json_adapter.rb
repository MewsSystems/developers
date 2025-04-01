require 'json'
require_relative '../base/base_adapter'
require_relative '../utilities/content_type_detector'

class JsonAdapter < BaseAdapter
  CONTENT_TYPES = Adapters::Utilities::ContentTypeDetector::JSON_CONTENT_TYPES

  def supports_content_type?(content_type)
    Adapters::Utilities::ContentTypeDetector.is_json_content_type?(content_type)
  end

  def supports_content?(content)
    Adapters::Utilities::ContentTypeDetector.looks_like_json?(content)
  end

  # Parse JSON exchange rate data
  # Expected formats:
  #
  # Format 1 (object with currency codes as keys):
  # {
  #   "date": "2023-01-01",
  #   "base": "CZK",
  #   "rates": {
  #     "USD": 0.043,
  #     "EUR": 0.039,
  #     ...
  #   }
  # }
  #
  # Format 2 (array of rate objects):
  # {
  #   "date": "2023-01-01",
  #   "base": "CZK",
  #   "rates": [
  #     {"code": "USD", "rate": 0.043, "amount": 1},
  #     {"code": "EUR", "rate": 0.039, "amount": 1},
  #     ...
  #   ]
  # }
  def perform_parse(data, base_currency)
    begin
      # Parse JSON
      parsed_data = JSON.parse(data)
    rescue => e
      raise ParseError, "Invalid JSON: #{e.message}"
    end

    rates = []

    # Get date from JSON or use today
    date = extract_date(parsed_data['date'] ||
                        parsed_data['effectiveDate'] ||
                        parsed_data['effective_date'])

    # Try to get base currency from JSON if provided
    json_base = parsed_data['base'] || parsed_data['baseCurrency'] || parsed_data['base_currency']
    base_currency = standardize_currency_code(json_base) if json_base

    # Try different JSON formats
    rates = try_rates_object_format(parsed_data, base_currency, date) ||
            try_rates_array_format(parsed_data, base_currency, date) ||
            try_direct_array_format(parsed_data, base_currency, date) ||
            try_direct_object_format(parsed_data, base_currency, date) ||
            []

    rates
  end

  private

  # Extract date from JSON data
  # @param data [Hash] Parsed JSON data
  # @return [Date, nil] Extracted date or nil if not found
  def extract_date(data)
    date_str = data['date'] || data['effectiveDate'] || data['effective_date']
    return nil unless date_str

    begin
      Date.parse(date_str.to_s)
    rescue
      nil
    end
  end

  # Parse rate value from various formats
  # @param value [Object] Value to parse
  # @return [Float] Parsed rate value
  def parse_rate_value(value)
    if value.is_a?(Numeric)
      value.to_f
    elsif value.is_a?(String)
      value.gsub(',', '.').to_f
    elsif value.is_a?(Hash) && (value['value'] || value['rate'])
      (value['value'] || value['rate']).to_f
    else
      0.0
    end
  end

  # Format 1: Object with currency codes as keys
  def try_rates_object_format(parsed_data, base_currency, date)
    return nil unless parsed_data['rates']&.is_a?(Hash)

    rates = []
    parsed_data['rates'].each do |code, value|
      currency_code = standardize_currency_code(code)

      # Skip invalid codes
      next if currency_code.empty? || currency_code.length != 3

      rate_value = parse_rate_value(value)
      next if rate_value.zero?

      rates << create_exchange_rate(base_currency, currency_code, rate_value, 1, date)
    end

    rates.empty? ? nil : rates
  end

  # Format 2: Array of rate objects
  def try_rates_array_format(parsed_data, base_currency, date)
    return nil unless parsed_data['rates']&.is_a?(Array)

    rates = []
    parsed_data['rates'].each do |rate_obj|
      next unless rate_obj.is_a?(Hash)

      code = rate_obj['code'] || rate_obj['currency'] || rate_obj['currencyCode']
      next unless code

      currency_code = standardize_currency_code(code)

      # Skip invalid codes
      next if currency_code.empty? || currency_code.length != 3

      rate_value = parse_rate_value(rate_obj['rate'] || rate_obj['value'])
      next if rate_value.zero?

      amount = (rate_obj['amount'] || 1).to_i

      rates << create_exchange_rate(base_currency, currency_code, rate_value, amount, date)
    end

    rates.empty? ? nil : rates
  end

  # Format 3: Direct array of currency objects
  def try_direct_array_format(parsed_data, base_currency, date)
    return nil unless parsed_data.is_a?(Array)

    rates = []
    parsed_data.each do |rate_obj|
      next unless rate_obj.is_a?(Hash)

      code = rate_obj['code'] || rate_obj['currency'] || rate_obj['currencyCode']
      next unless code

      currency_code = standardize_currency_code(code)

      # Skip invalid codes
      next if currency_code.empty? || currency_code.length != 3

      rate_value = parse_rate_value(rate_obj['rate'] || rate_obj['value'])
      next if rate_value.zero?

      amount = (rate_obj['amount'] || 1).to_i

      rates << create_exchange_rate(base_currency, currency_code, rate_value, amount, date)
    end

    rates.empty? ? nil : rates
  end

  # Format 4: Direct object with currency codes as keys
  def try_direct_object_format(parsed_data, base_currency, date)
    return nil unless parsed_data.is_a?(Hash)

    rates = []
    parsed_data.each do |code, value|
      # Skip non-currency fields
      next if ['date', 'base', 'baseCurrency', 'base_currency'].include?(code)

      currency_code = standardize_currency_code(code)

      # Skip invalid codes
      next if currency_code.empty? || currency_code.length != 3

      rate_value = parse_rate_value(value)
      next if rate_value.zero?

      rates << create_exchange_rate(base_currency, currency_code, rate_value, 1, date)
    end

    rates.empty? ? nil : rates
  end
end