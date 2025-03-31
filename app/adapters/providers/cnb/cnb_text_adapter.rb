require_relative '../../base/base_adapter'
require_relative '../../../errors/exchange_rate_errors'

module Adapters
  module Strategies
    class CnbTextAdapter < BaseAdapter
      # The encoding used by CNB's text feed (ISO-8859-2 / Latin-2)
      SOURCE_ENCODING = 'ISO-8859-2'
      
      def initialize(provider_name = 'CNB')
        super(provider_name)
      end

      # Check if this adapter supports the given content type
      def supports_content_type?(content_type)
        content_type && (content_type.downcase.include?('text/plain') || 
                         content_type.downcase.include?('text/csv') ||
                         content_type.downcase.include?('text/txt'))
      end

      # Check if this adapter supports the given content
      def supports_content?(content)
        return false unless content
        content = content.to_s
        # Check for characteristic patterns in CNB text data (e.g. pipe delimited)
        has_pipes = content.include?('|')
        has_currency_codes = content =~ /[A-Z]{3}/
        has_pipes && has_currency_codes
      end
      
      # Parse CNB text feed format
      # @param data [String] Raw text data from CNB
      # @param base_currency_code [String] Base currency code (e.g., 'CZK')
      # @return [Array<ExchangeRate>] Array of exchange rate domain objects
      def perform_parse(data, base_currency_code)
        # Handle encoding - CNB uses ISO-8859-2
        data = ensure_utf8_encoding(data, SOURCE_ENCODING)
        
        lines = data.split("\n")
        raise ExchangeRateErrors::ParseError.new("Empty data from CNB feed", nil, @provider_name) if lines.empty?

        # First line contains date and order number (e.g., "26.03.2025 #60")
        header_line_index = 1
        if lines.size <= header_line_index
          raise ExchangeRateErrors::ParseError.new("CNB feed data missing header line", nil, @provider_name)
        end

        header = lines[header_line_index].strip
        expected_header = "Country|Currency|Amount|Code|Rate"
        unless header == expected_header
          raise ExchangeRateErrors::ParseError.new("Unexpected header format in CNB feed: '#{header}'", nil, @provider_name)
        end

        # Prepare domain objects
        rates = []
        currency_map = {}  # to reuse Currency instances by code
        base_currency = currency_map[base_currency_code] ||= create_currency(base_currency_code)

        # CNB feed lines format: Country|Currency|Amount|Code|Rate
        # e.g., "Australia|dollar|1|AUD|14.602" meaning 1 AUD = 14.602 CZK.
        lines[(header_line_index + 1)..-1].each do |line|
          line = line.strip
          next if line.empty?  # blank line indicates end of main rates section
          
          parts = line.split('|')
          unless parts.size == 5
            raise ExchangeRateErrors::ParseError.new("Malformed line in CNB feed: '#{line}'", nil, @provider_name)
          end
          
          country, currency_name, amount_str, code, rate_str = parts
          
          # Parse amount and rate
          amount = parse_amount(amount_str, code)
          rate = parse_rate(rate_str, code)
          
          # Create domain Currency for this code if not already created
          currency_obj = currency_map[code] ||= create_currency(code, currency_name)
          
          # Create ExchangeRate from base currency to this currency with the normalized rate
          rates << create_exchange_rate(base_currency_code, code, rate, amount)
        end

        if rates.empty?
          raise ExchangeRateErrors::ParseError.new("No exchange rate data found in CNB feed", nil, @provider_name)
        end

        rates
      end
    end
  end
end 