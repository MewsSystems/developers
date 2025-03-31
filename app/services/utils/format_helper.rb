module Utils
  # Helper for consistent formatting and parsing operations
  # Centralizes common data formatting functions
  module FormatHelper
    # Extract date from a string in various formats
    # @param date_str [String] String to extract date from
    # @return [Date, nil] Extracted date or nil if not found
    def self.extract_date(date_str)
      return nil unless date_str
      
      begin
        # Try to parse standard ISO format
        return Date.parse(date_str.to_s)
      rescue
        # Try to match DD.MM.YYYY format
        if match = date_str.to_s.match(/(\d{1,2})\.(\d{1,2})\.(\d{4})/)
          day, month, year = match.captures
          return Date.new(year.to_i, month.to_i, day.to_i)
        end
        
        # Try to match YYYY-MM-DD format
        if match = date_str.to_s.match(/(\d{4})-(\d{1,2})-(\d{1,2})/)
          year, month, day = match.captures
          return Date.new(year.to_i, month.to_i, day.to_i)
        end
        
        # Return nil if no match found
        nil
      end
    end
    
    # Parse rate value from various formats to float
    # @param value [Object] Value to parse
    # @return [Float] Parsed rate value
    def self.parse_rate_value(value)
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
    
    # Standardize a currency code
    # @param code [String] Currency code
    # @return [String] Standardized currency code
    def self.standardize_currency_code(code)
      code.to_s.strip.upcase
    end
    
    # Parse amount value from either dedicated field or use default
    # @param amount_str [String, nil] Amount string
    # @param currency_code [String] Currency code (for context)
    # @return [Integer] Amount value (default: 1)
    def self.parse_amount(amount_str, currency_code = nil)
      return 1 unless amount_str
      
      # Extract numeric value
      amount = amount_str.to_s.strip
      
      # Handle special case for Japanese Yen and similar
      if currency_code && ['JPY', 'IDR'].include?(currency_code.to_s.upcase)
        # These currencies often use 100 units as base in exchange rates
        return amount.empty? ? 100 : amount.to_i
      end
      
      # Default
      amount.empty? ? 1 : amount.to_i
    end
    
    # Convert data to UTF-8 if needed
    # @param data [String] Raw data in source encoding
    # @param source_encoding [String] Source encoding of the data
    # @return [String] Data converted to UTF-8
    def self.ensure_utf8_encoding(data, source_encoding = 'UTF-8')
      return data if source_encoding == 'UTF-8' || data.encoding.name == 'UTF-8'
      
      begin
        data.encode('UTF-8', source_encoding)
      rescue Encoding::UndefinedConversionError, Encoding::InvalidByteSequenceError => e
        raise "Encoding conversion error: #{e.message}"
      end
    end
    
    # Content type detection helpers
    module ContentTypeDetection
      XML_CONTENT_TYPES = [
        'text/xml',
        'application/xml',
        'application/xhtml+xml'
      ].freeze
      
      JSON_CONTENT_TYPES = [
        'application/json',
        'text/json'
      ].freeze
      
      TXT_CONTENT_TYPES = [
        'text/plain',
        'text/txt',
        'application/txt',
        'text/csv'
      ].freeze
      
      def self.is_xml_content_type?(content_type)
        return false unless content_type
        XML_CONTENT_TYPES.any? { |type| content_type.to_s.downcase.include?(type) }
      end
      
      def self.is_json_content_type?(content_type)
        return false unless content_type
        JSON_CONTENT_TYPES.any? { |type| content_type.to_s.downcase.include?(type) }
      end
      
      def self.is_txt_content_type?(content_type)
        return false unless content_type
        TXT_CONTENT_TYPES.any? { |type| content_type.to_s.downcase.include?(type) }
      end
      
      def self.looks_like_xml?(content)
        return false unless content
        content = content.to_s.strip
        content.start_with?('<?xml') || content.match?(/<[a-zA-Z][^>]*>/)
      end
      
      def self.looks_like_json?(content)
        return false unless content
        content = content.to_s.strip
        (content.start_with?('{') && content.end_with?('}')) ||
          (content.start_with?('[') && content.end_with?(']'))
      end
    end
  end
end 