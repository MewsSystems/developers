require 'json'
require_relative '../../base/base_adapter'
require_relative '../../../errors/exchange_rate_errors'

module Adapters
  module Strategies
    class CnbJsonAdapter < BaseAdapter
      def initialize(provider_name = 'CNB')
        super(provider_name)
      end

      # Check if this adapter supports the given content type
      def supports_content_type?(content_type)
        content_type && content_type.downcase.include?('json')
      end

      # Check if this adapter supports the given content
      def supports_content?(content)
        return false unless content
        content = content.to_s.strip
        content.start_with?('{') || content.start_with?('[')
      end

      # Parse CNB JSON feed format
      # @param data [String] Raw JSON data from CNB
      # @param base_currency_code [String] Base currency code (e.g., 'CZK')
      # @return [Array<ExchangeRate>] Array of exchange rate domain objects
      def perform_parse(data, base_currency_code)
        begin
          # Parse JSON
          parsed_data = JSON.parse(data)
        rescue JSON::ParserError => e
          raise ExchangeRateErrors::ParseError.new(
            "Error parsing CNB JSON feed: #{e.message}",
            e, @provider_name
          )
        end

        # CNB JSON format can vary, but typically it's an object with
        # a 'rates' property containing an array of rate objects
        rates = []

        # Extract date - look in common locations
        date_str = parsed_data['date'] || parsed_data['datum']
        date = date_str ? extract_date(date_str) : Date.today

        # Try different JSON structures based on what CNB might provide

        # Format 1: A single object with currency codes as properties
        if parsed_data.is_a?(Hash) && parsed_data.keys.any? { |k| k.to_s.length == 3 }
          parsed_data.each do |code, value|
            # Skip non-currency fields
            next unless code.to_s.length == 3

            currency_code = standardize_currency_code(code)

            # Extract rate and amount
            if value.is_a?(Hash)
              rate_value = parse_rate_value(value['rate'] || value['kurz'] || value['value'])
              amount = (value['amount'] || value['mnozstvi'] || 1).to_i
            else
              rate_value = parse_rate_value(value)
              amount = 1
            end

            next if rate_value.zero?

            # Create exchange rate object
            rates << create_exchange_rate(base_currency_code, currency_code, rate_value, amount, date)
          end
        end

        # Format 2: Array of objects with currency details
        if rates.empty? && parsed_data.is_a?(Array)
          parsed_data.each do |item|
            next unless item.is_a?(Hash)

            code = item['code'] || item['kod']
            next unless code

            currency_code = standardize_currency_code(code)

            # Extract rate and amount
            rate_value = parse_rate_value(item['rate'] || item['kurz'] || item['value'])
            amount = (item['amount'] || item['mnozstvi'] || 1).to_i

            next if rate_value.zero?

            # Create exchange rate object
            rates << create_exchange_rate(base_currency_code, currency_code, rate_value, amount, date)
          end
        end

        # Format 3: Object with 'rates' array property
        if rates.empty? && parsed_data['rates'].is_a?(Array)
          parsed_data['rates'].each do |item|
            next unless item.is_a?(Hash)

            code = item['code'] || item['kod']
            next unless code

            currency_code = standardize_currency_code(code)

            # Extract rate and amount
            rate_value = parse_rate_value(item['rate'] || item['kurz'] || item['value'])
            amount = (item['amount'] || item['mnozstvi'] || 1).to_i

            next if rate_value.zero?

            # Create exchange rate object
            rates << create_exchange_rate(base_currency_code, currency_code, rate_value, amount, date)
          end
        end

        # Format 4: Object with 'rates' object property
        if rates.empty? && parsed_data['rates'].is_a?(Hash)
          parsed_data['rates'].each do |code, value|
            currency_code = standardize_currency_code(code)

            # Extract rate and amount
            if value.is_a?(Hash)
              rate_value = parse_rate_value(value['rate'] || value['kurz'] || value['value'])
              amount = (value['amount'] || value['mnozstvi'] || 1).to_i
            else
              rate_value = parse_rate_value(value)
              amount = 1
            end

            next if rate_value.zero?

            # Create exchange rate object
            rates << create_exchange_rate(base_currency_code, currency_code, rate_value, amount, date)
          end
        end

        if rates.empty?
          raise ExchangeRateErrors::ParseError.new(
            "No valid exchange rates found in CNB JSON feed",
            nil, @provider_name
          )
        end

        rates
      end
    end
  end
end