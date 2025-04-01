module Adapters
  module Utilities
    # This module has been deprecated.
    # Use Utils::FormatHelper instead.
    module FormatUtilities
      # This module is deprecated. Please use Utils::FormatHelper instead.
      def self.method_missing(method_name, *args, &block)
        if Utils::FormatHelper.respond_to?(method_name)
          Utils::FormatHelper.send(method_name, *args, &block)
        else
          super
        end
      end

      def self.respond_to_missing?(method_name, include_private = false)
        Utils::FormatHelper.respond_to?(method_name) || super
      end

      # Forward the ContentTypeDetection module
      ContentTypeDetection = Utils::FormatHelper::ContentTypeDetection

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
    end
  end
end