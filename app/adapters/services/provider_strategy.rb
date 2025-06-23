require_relative '../registry/adapter_registry'
require_relative '../utilities/content_type_detector'

module Adapters
  module Services
    # Strategy for handling provider-specific adapter selection
    class ProviderStrategy
      # Get a provider-specific adapter for the given format
      # @param provider_name [String] Provider name
      # @param format [String] Format name
      # @return [Class, nil] Adapter class or nil if not found
      def self.get_provider_adapter(provider_name, format)
        registry = Adapters::AdapterRegistry.instance
        registry.provider_adapter(provider_name, format)
      end

      # Find the best adapter for a given provider and file extension
      # @param provider_name [String] Provider name
      # @param file_extension [String] File extension
      # @return [Class, nil] Adapter class or nil if not found
      def self.adapter_for_extension(provider_name, file_extension)
        ext = file_extension.to_s.downcase.delete('.')
        format = Utilities::ContentTypeDetector.format_from_extension(ext)
        format ? get_provider_adapter(provider_name, format.to_s) : nil
      end

      # Find the best adapter for a given provider and content type
      # @param provider_name [String] Provider name
      # @param content_type [String] Content type
      # @return [Class, nil] Adapter class or nil if not found
      def self.adapter_for_content_type(provider_name, content_type)
        return nil if content_type.nil?

        format = Utilities::ContentTypeDetector.format_from_content_type(content_type)
        format ? get_provider_adapter(provider_name, format.to_s) : nil
      end

      # Find the best adapter for the content itself
      # @param provider_name [String] Provider name
      # @param content [String] Content to analyze
      # @return [Class, nil] Adapter class or nil if not found
      def self.adapter_for_content(provider_name, content)
        content_str = content.to_s
        format = Utilities::ContentTypeDetector.detect_format(content_str)
        get_provider_adapter(provider_name, format.to_s)
      end

      # Get the default text adapter for a provider
      # @param provider_name [String] Provider name
      # @return [Class] Text adapter class
      def self.default_text_adapter(provider_name)
        registry = Adapters::AdapterRegistry.instance

        # For CNB provider, use CnbTextAdapter for backward compatibility
        if provider_name == 'CNB'
          adapter_class = registry.provider_adapter(provider_name, 'txt')
          return adapter_class if adapter_class
        end

        # For other providers, use standard TxtAdapter
        registry.standard_adapters.find { |cls| cls.name.include?('TxtAdapter') }
      end
    end
  end
end