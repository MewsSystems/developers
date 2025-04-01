require_relative '../utilities/content_type_detector'
require_relative '../registry/adapter_registry'
require_relative '../utilities/error_handler'
require_relative 'provider_strategy'
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
        adapter_class = ProviderStrategy.adapter_for_extension(provider_name, ext)
        return create_adapter_instance(adapter_class, provider_name) if adapter_class
        
        # Try extension adapter
        adapter_class = registry.extension_adapter(ext)
        return create_adapter_instance(adapter_class, provider_name) if adapter_class
        
        # No adapter found
        Utilities::ErrorHandler.raise_unsupported_format_error(
          provider_name, "file extension", file_extension
        )
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
          adapter_class = ProviderStrategy.default_text_adapter(provider_name)
          return create_adapter_instance(adapter_class, provider_name)
        end
        
        # Try provider-specific adapter for content type
        adapter_class = ProviderStrategy.adapter_for_content_type(provider_name, content_type)
        return create_adapter_instance(adapter_class, provider_name) if adapter_class
        
        # Try each standard adapter
        registry.standard_adapters.each do |adapter_class|
          adapter = adapter_class.new(nil) # Temporary instance to check compatibility
          return create_adapter_instance(adapter_class, provider_name) if adapter.supports_content_type?(content_type)
        end
        
        # No adapter found
        Utilities::ErrorHandler.raise_unsupported_format_error(
          provider_name, 'content type', content_type
        )
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
          adapter_class = for_file_extension_internal(provider_name, file_extension)
          return adapter_class if adapter_class
        end
        
        # Convert content to string for inspection
        content_str = content.to_s
        
        # Check provider-specific content handlers
        adapter_class = ProviderStrategy.adapter_for_content(provider_name, content_str)
        return create_adapter_instance(adapter_class, provider_name) if adapter_class
        
        # Try standard adapters based on content detection
        registry.standard_adapters.each do |adapter_class|
          adapter = adapter_class.new(nil) # Temporary instance to check compatibility
          return create_adapter_instance(adapter_class, provider_name) if adapter.supports_content?(content_str)
        end
        
        # Fallback to text adapter
        adapter_class = ProviderStrategy.default_text_adapter(provider_name)
        create_adapter_instance(adapter_class, provider_name)
      end
      
      private
      
      # Internal method to avoid raising errors when looking up by file extension for the content method
      # @param provider_name [String] Provider name
      # @param file_extension [String] File extension
      # @return [BaseAdapter, nil] Adapter or nil if not found
      def self.for_file_extension_internal(provider_name, file_extension)
        ext = file_extension.to_s.downcase.delete('.')
        registry = Adapters::AdapterRegistry.instance
        
        # Try provider-specific adapter first
        adapter_class = ProviderStrategy.adapter_for_extension(provider_name, ext)
        return create_adapter_instance(adapter_class, provider_name) if adapter_class
        
        # Try extension adapter
        adapter_class = registry.extension_adapter(ext)
        return create_adapter_instance(adapter_class, provider_name) if adapter_class
        
        nil
      end
      
      # Create an adapter instance
      # @param adapter_class [Class] Adapter class
      # @param provider_name [String] Provider name
      # @return [BaseAdapter] Adapter instance
      def self.create_adapter_instance(adapter_class, provider_name)
        adapter_class.new(provider_name)
      end
    end
  end
end 