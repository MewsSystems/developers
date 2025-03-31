require 'nokogiri'
require_relative '../../base/base_adapter'
require_relative '../../../errors/exchange_rate_errors'

module Adapters
  module Strategies
    class CnbXmlAdapter < BaseAdapter
      # CNB XML feed also uses ISO-8859-2
      SOURCE_ENCODING = 'ISO-8859-2'
      
      def initialize(provider_name = 'CNB')
        super(provider_name)
      end

      # Check if this adapter supports the given content type
      def supports_content_type?(content_type)
        return false unless content_type
        content_type.downcase.include?('xml')
      end

      # Check if this adapter supports the given content
      def supports_content?(content)
        return false unless content
        content = content.to_s.strip
        content.start_with?('<?xml') || content.start_with?('<kurzy')
      end
      
      # Parse CNB XML feed format
      # @param data [String] Raw XML data from CNB
      # @param base_currency_code [String] Base currency code (e.g., 'CZK')
      # @return [Array<ExchangeRate>] Array of exchange rate domain objects
      def perform_parse(data, base_currency_code)
        begin
          # Parse XML
          doc = Nokogiri::XML(data) { |config| config.strict }
        rescue => e
          raise ExchangeRateErrors::ParseError.new(
            "Error parsing CNB XML feed: #{e.message}",
            e, @provider_name
          )
        end
        
        # Handle possible root elements
        # CNB typically uses <kurzy> as the root
        root = doc.root
        raise ExchangeRateErrors::ParseError.new(
          "Invalid CNB XML format: no root element", 
          nil, @provider_name
        ) unless root
        
        # Extract date - CNB uses 'datum' attribute
        date_str = root['datum'] || root['date']
        date = date_str ? extract_date(date_str) : Date.today
        
        rates = []
        
        # Process <radek> elements (CNB format)
        root.css('radek').each do |row|
          begin
            country = row['zeme']
            currency_name = row['mena']
            code = standardize_currency_code(row['kod'])
            rate_str = row['kurz']
            amount_str = row['mnozstvi']
            
            # Skip invalid codes
            next if code.empty? || code.length != 3
            
            # Parse amount and rate
            amount = amount_str ? parse_amount(amount_str, code) : 1
            rate = parse_rate(rate_str, code)
            
            # Create exchange rate object
            rates << create_exchange_rate(base_currency_code, code, rate, amount, date)
          rescue => e
            # Log error and continue with next element
            log_error("Error processing CNB XML rate element: #{e.message}")
            next
          end
        end
        
        # Fallback: try generic format if no rates found
        if rates.empty?
          # Try other common XML formats
          rates = try_generic_xml_format(doc, base_currency_code, date)
        end
        
        if rates.empty?
          raise ExchangeRateErrors::ParseError.new(
            "No valid exchange rates found in CNB XML feed",
            nil, @provider_name
          )
        end
        
        rates
      end
      
      private
      
      # Try parsing generic XML format as fallback
      def try_generic_xml_format(doc, base_currency, date)
        rates = []
        
        # Try to find currency elements by common patterns
        currency_elements = doc.css('kurz, rate, currency')
        
        currency_elements.each do |elem|
          begin
            code = elem['kod'] || elem['code'] || elem['currency']
            next unless code
            
            code = standardize_currency_code(code)
            next if code.empty? || code.length != 3
            
            rate_str = elem.text.strip
            rate_str = elem['rate'] || elem['value'] || elem['kurz'] if rate_str.empty?
            next if rate_str.to_s.empty?
            
            amount_str = elem['amount'] || elem['mnozstvi'] || "1"
            
            # Parse values
            amount = parse_amount(amount_str, code)
            rate = parse_rate(rate_str, code)
            
            # Create exchange rate object
            rates << create_exchange_rate(base_currency, code, rate, amount, date)
          rescue => e
            # Skip problematic elements
            next
          end
        end
        
        rates
      end
    end
  end
end 