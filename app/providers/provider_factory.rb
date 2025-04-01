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
    provider_type = normalize_provider_type(provider_type)
    class_name = get_provider_class_name(provider_type)
    load_provider_class(provider_type, class_name)
    
    # Get the class and create an instance
    provider_class = Object.const_get(class_name)
    provider_class.new(config)
  end

  # Create a provider instance based on application settings
  # @return [BaseProvider] Provider instance
  def self.create_provider
    settings = Rails.configuration.settings
    provider_type = get_provider_type_from_settings(settings)
    provider_config = get_provider_config_from_settings(settings, provider_type)
    
    create(provider_type, provider_config)
  end

  # Get a list of available provider types
  # @return [Array<String>] List of available provider type identifiers
  def self.available_providers
    PROVIDER_CLASSES.keys
  end
  
  private
  
  # Normalize provider type to lowercase string
  # @param provider_type [String, Symbol] Provider type
  # @return [String] Normalized provider type
  def self.normalize_provider_type(provider_type)
    provider_type.to_s.downcase
  end
  
  # Get the class name for a provider type
  # @param provider_type [String] Provider type
  # @return [String] Provider class name
  # @raise [ProviderNotFoundError] If provider type is not supported
  def self.get_provider_class_name(provider_type)
    class_name = PROVIDER_CLASSES[provider_type]
    
    unless class_name
      available = PROVIDER_CLASSES.keys.join(', ')
      raise ProviderNotFoundError, "No provider found for type: #{provider_type}. " \
                                  "Available providers: #{available}"
    end
    
    class_name
  end
  
  # Load the provider class if not already loaded
  # @param provider_type [String] Provider type
  # @param class_name [String] Provider class name
  def self.load_provider_class(provider_type, class_name)
    unless Object.const_defined?(class_name)
      require_relative provider_type + '_provider'
    end
  end
  
  # Get provider type from settings
  # @param settings [Hash] Application settings
  # @return [String] Provider type
  def self.get_provider_type_from_settings(settings)
    provider_type = settings['provider'] || settings[:provider]
    normalize_provider_type(provider_type)
  end
  
  # Get provider configuration from settings
  # @param settings [Hash] Application settings
  # @param provider_type [String] Provider type
  # @return [Hash] Provider configuration
  def self.get_provider_config_from_settings(settings, provider_type)
    provider_config = settings['providers'] || settings[:providers] || {}
    
    provider_config[provider_type.upcase] ||
    provider_config[provider_type.to_sym] ||
    provider_config[provider_type] ||
    {}
  end
end