class ProviderFactory
  # Map of provider name to provider class
  PROVIDER_CLASSES = {
    'cnb' => 'CNBProvider',
    'ecb' => 'ECBProvider',
    # Add more providers here as they are implemented
  }.freeze
  
  class ProviderNotFoundError < StandardError; end
  
  # Create a provider instance based on configuration
  # @param provider_type [String, Symbol] Provider type identifier
  # @param config [Hash] Configuration for the provider
  # @return [BaseProvider] Provider instance
  def self.create(provider_type, config = {})
    provider_type = provider_type.to_s.downcase
    
    # Get the class name for this provider
    class_name = PROVIDER_CLASSES[provider_type]
    
    unless class_name
      raise ProviderNotFoundError, "No provider found for type: #{provider_type}. " \
                                  "Available providers: #{PROVIDER_CLASSES.keys.join(', ')}"
    end
    
    # Load the provider class if not already loaded
    unless Object.const_defined?(class_name)
      require_relative provider_type + '_provider'
    end
    
    # Get the class and create an instance
    provider_class = Object.const_get(class_name)
    provider_class.new(config)
  end

  # Create a provider instance based on application settings
  # @return [BaseProvider] Provider instance
  def self.create_provider
    settings = Rails.configuration.settings
    
    # Handle case sensitivity by normalizing the provider name
    provider_type = settings['provider'] || settings[:provider]
    provider_type = provider_type.to_s.downcase
    
    # Get provider configuration from settings
    provider_config = settings['providers'] || settings[:providers]
    provider_config = provider_config[provider_type.upcase] || provider_config[provider_type.to_sym] || provider_config[provider_type] || {}
    
    # Create provider with the configuration
    create(provider_type, provider_config)
  end
  
  # Get a list of available provider types
  # @return [Array<String>] List of available provider type identifiers
  def self.available_providers
    PROVIDER_CLASSES.keys
  end
end 