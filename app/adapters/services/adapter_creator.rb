require_relative '../utilities/content_type_detector'
require_relative '../registry/adapter_registry'
require_relative '../../errors/exchange_rate_errors'

module Adapters
  module Services
    # Service for creating adapter instances
    class AdapterCreator
      # Create an adapter based on file extension
      # @param provider_name [String] Name of the provider
      # @param file_extension [String] File extension
      # @return [BaseAdapter] Appropriate adapter for the file extension
      # @raise [ExchangeRateErrors::UnsupportedFormatError] If no adapter supports the file extension
      def self.for_file_extension(provider_name, file_extension)
        registry = Adapters::AdapterRegistry.instance
        registry.validate_provider_support(provider_name)
        
        ext = file_extension.to_s.downcase.delete('.')
        
        # Try to find provider-specific adapter first
        format = Utilities::ContentTypeDetector.format_from_extension(ext)
        if format && (adapter_class = registry.provider_adapter(provider_name, format.to_s))
          return create_adapter_instance(adapter_class, provider_name)
        end
        
        # Try extension adapter
        if adapter_class = registry.extension_adapter(ext)
          return create_adapter_instance(adapter_class, provider_name)
        end
        
        # No adapter found
        raise_unsupported_format_error(provider_name, "file extension", file_extension)
      end
      
      # Create an adapter by content type
      # @param provider_name [String] Name of the provider
      # @param content_type [String] Content type header
      # @return [BaseAdapter] Appropriate adapter for the content type
      # @raise [ExchangeRateErrors::UnsupportedFormatError] If no adapter supports the content type
      def self.for_content_type(provider_name, content_type)
        registry = Adapters::AdapterRegistry.instance
        registry.validate_provider_support(provider_name)
        
        # Default to text adapter when content_type is nil
        if content_type.nil?
          return create_default_text_adapter(provider_name)
        end
        
        # Try provider-specific adapter for content type
        format = Utilities::ContentTypeDetector.format_from_content_type(content_type)
        if format && (adapter_class = registry.provider_adapter(provider_name, format.to_s))
          return create_adapter_instance(adapter_class, provider_name)
        end
        
        # For CNB provider, use special error handling for backward compatibility
        if provider_name == 'CNB'
          raise_unsupported_format_error(provider_name, 'content type', content_type)
        end
        
        # Try each standard adapter
        registry.standard_adapters.each do |adapter_class|
          adapter = adapter_class.new(nil) # Temporary instance to check compatibility
          return create_adapter_instance(adapter_class, provider_name) if adapter.supports_content_type?(content_type)
        end
        
        # No adapter found
        raise_unsupported_format_error(provider_name, 'content type', content_type)
      end
      
      # Create an adapter by inspecting the content
      # @param provider_name [String] Name of the provider
      # @param content [String] Content to inspect
      # @param file_extension [String] Optional file extension hint
      # @return [BaseAdapter] Appropriate adapter for the content
      # @raise [ExchangeRateErrors::UnsupportedFormatError] If no adapter supports the content
      def self.for_content(provider_name, content, file_extension = nil)
        registry = Adapters::AdapterRegistry.instance
        registry.validate_provider_support(provider_name)
        
        # Try by file extension first if provided
        if file_extension
          ext = file_extension.to_s.downcase.delete('.')
          format = Utilities::ContentTypeDetector.format_from_extension(ext)
          
          if format && (adapter_class = registry.provider_adapter(provider_name, format.to_s))
            return create_adapter_instance(adapter_class, provider_name)
          elsif adapter_class = registry.extension_adapter(ext)
            return create_adapter_instance(adapter_class, provider_name)
          end
        end
        
        # Convert content to string for inspection
        content_str = content.to_s
        
        # Check provider-specific content handlers first
        if provider_name == 'CNB'
          format = Utilities::ContentTypeDetector.detect_format(content_str)
          adapter_class = registry.provider_adapter(provider_name, format.to_s)
          return create_adapter_instance(adapter_class, provider_name) if adapter_class
        end
        
        # Try standard adapters based on content detection
        format = Utilities::ContentTypeDetector.detect_format(content_str)
        registry.standard_adapters.each do |adapter_class|
          adapter = adapter_class.new(nil) # Temporary instance to check compatibility
          return create_adapter_instance(adapter_class, provider_name) if adapter.supports_content?(content_str)
        end
        
        # Fallback to text adapter
        return create_default_text_adapter(provider_name)
      end
      
      private
      
      # Create an adapter instance
      # @param adapter_class [Class] Adapter class
      # @param provider_name [String] Provider name
      # @return [BaseAdapter] Adapter instance
      def self.create_adapter_instance(adapter_class, provider_name)
        adapter_class.new(provider_name)
      end
      
      # Create the default text adapter for a provider
      # @param provider_name [String] Provider name
      # @return [BaseAdapter] Text adapter instance
      def self.create_default_text_adapter(provider_name)
        registry = Adapters::AdapterRegistry.instance
        
        # For CNB provider, use CnbTextAdapter for backward compatibility
        if provider_name == 'CNB'
          adapter_class = registry.provider_adapter(provider_name, 'txt')
          return create_adapter_instance(adapter_class, provider_name) if adapter_class
        end
        
        # For other providers, use standard TxtAdapter
        adapter_class = registry.standard_adapters.find { |cls| cls.name.include?('TxtAdapter') }
        create_adapter_instance(adapter_class, provider_name)
      end
      
      # Raise appropriate error for unsupported format
      # @param provider_name [String] Provider name
      # @param format_type [String] Format type description (e.g., "file extension", "content type")
      # @param format_value [String] Actual format value
      # @raise [ExchangeRateErrors::UnsupportedFormatError] Standardized error
      def self.raise_unsupported_format_error(provider_name, format_type, format_value, context = {})
        # Special case for CNB provider with content types for backward compatibility
        if provider_name == 'CNB' && format_type == 'content type'
          raise BaseAdapter::UnsupportedFormatError.new("Content type '#{format_value}' not supported")
        end
        
        context_hash = { format_type.gsub(' ', '_') => format_value }
        context_hash.merge!(context) if context.is_a?(Hash)
        
        raise ExchangeRateErrors::UnsupportedFormatError.new(
          "No adapter found for #{format_type}: #{format_value}",
          nil, provider_name, context_hash
        )
      end
    end
  end
end 