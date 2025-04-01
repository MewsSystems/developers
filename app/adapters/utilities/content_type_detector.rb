require 'json'

module Adapters
  module Utilities
    # Utility for detecting content types from data
    class ContentTypeDetector
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

      # Check if content type is XML
      # @param content_type [String] Content type to check
      # @return [Boolean] Whether the content type is XML
      def self.is_xml_content_type?(content_type)
        return false unless content_type
        XML_CONTENT_TYPES.any? { |type| content_type.to_s.downcase.include?(type) }
      end

      # Check if content type is JSON
      # @param content_type [String] Content type to check
      # @return [Boolean] Whether the content type is JSON
      def self.is_json_content_type?(content_type)
        return false unless content_type
        JSON_CONTENT_TYPES.any? { |type| content_type.to_s.downcase.include?(type) }
      end

      # Check if content type is text
      # @param content_type [String] Content type to check
      # @return [Boolean] Whether the content type is text
      def self.is_txt_content_type?(content_type)
        return false unless content_type
        TXT_CONTENT_TYPES.any? { |type| content_type.to_s.downcase.include?(type) }
      end

      # Check if content looks like XML
      # @param content [String] Content to check
      # @return [Boolean] Whether the content looks like XML
      def self.looks_like_xml?(content)
        return false unless content
        content = content.to_s.strip
        content.start_with?('<?xml') || content.match?(/<[a-zA-Z][^>]*>/)
      end

      # Check if content looks like JSON
      # @param content [String] Content to check
      # @return [Boolean] Whether the content looks like JSON
      def self.looks_like_json?(content)
        return false unless content
        content = content.to_s.strip
        
        # Check structure
        if (content.start_with?('{') && content.end_with?('}')) ||
          (content.start_with?('[') && content.end_with?(']'))
          # Try to parse as JSON to confirm
          begin
            JSON.parse(content)
            return true
          rescue JSON::ParserError
            return false
          end
        end
        
        false
      end

      # Check if content looks like text
      # @param content [String] Content to check
      # @return [Boolean] Whether the content looks like text
      def self.looks_like_txt?(content)
        return false unless content
        !looks_like_xml?(content) && !looks_like_json?(content)
      end

      # Detect content format from content
      # @param content [String] Content to check
      # @return [Symbol] Format (:xml, :json, or :txt)
      def self.detect_format(content)
        if looks_like_xml?(content)
          :xml
        elsif looks_like_json?(content)
          :json
        else
          :txt
        end
      end

      # Detect format from file extension
      # @param extension [String] File extension
      # @return [Symbol, nil] Format or nil if unknown
      def self.format_from_extension(extension)
        ext = extension.to_s.downcase.delete('.')
        case ext
        when 'json'
          :json
        when 'xml'
          :xml
        when 'txt', 'text', 'csv'
          :txt
        else
          nil
        end
      end

      # Detect format from content type
      # @param content_type [String] Content type
      # @return [Symbol, nil] Format or nil if unknown
      def self.format_from_content_type(content_type)
        if is_json_content_type?(content_type)
          :json
        elsif is_xml_content_type?(content_type)
          :xml
        elsif is_txt_content_type?(content_type)
          :txt
        else
          nil
        end
      end
    end
  end
end 