module Utils
  module ProviderConfig
    # Add default provider configurations
    PROVIDER_DEFAULTS = {
      'CNB' => {
        base_currency: 'CZK',
        publication_hour: 14,
        publication_minute: 30,
        publication_timezone: '+01:00', # CET
        required_fields: ['base_url'],
        source_display_name: 'Czech National Bank (CNB)'
      },
      'ECB' => {
        base_currency: 'EUR',
        publication_hour: 16,
        publication_minute: 0,
        publication_timezone: '+01:00', # CET
        required_fields: ['base_url'],
        source_display_name: 'European Central Bank (ECB)'
      }
    }.freeze

    # Provider configuration DSL that simplifies configuration management
    # @param provider_name [String] The provider name (for error messages)
    # @param config [Hash] The provider configuration
    # @param overrides [Hash] Additional configuration overrides
    # @return [Hash] The merged configuration with defaults
    def self.configure(provider_name, config = {}, overrides = {})
      defaults = PROVIDER_DEFAULTS[provider_name] || {}
      required_fields = overrides[:required_fields] || defaults[:required_fields] || []
      
      # Merge defaults, then apply any overrides
      result = defaults.merge(config)
      result.merge!(overrides.reject { |k, _| k == :required_fields })
      
      # Validate required fields
      validate_required_fields(provider_name, result, required_fields)
      
      result
    end
    
    # Validate that required configuration fields are present
    # @param provider_name [String] The provider name (for error messages)
    # @param config [Hash] The provider configuration
    # @param required_fields [Array<String>] Required field names
    # @raise [ExchangeRateErrors::InvalidConfigurationError] If required fields are missing
    def self.validate_required_fields(provider_name, config, required_fields)
      missing_fields = required_fields.select { |field| config[field].nil? }
      
      if missing_fields.any?
        raise ExchangeRateErrors::InvalidConfigurationError.new(
          "#{provider_name} requires these configuration fields: #{missing_fields.join(', ')}",
          nil, provider_name
        )
      end
    end
    
    # Build standard provider metadata
    # @param options [Hash] Metadata options
    # @return [Hash] Standardized metadata hash
    def self.build_metadata(options)
      {
        source_name: options[:source_name],
        base_currency: options[:base_currency],
        publication_time: options[:publication_time]
      }
    end
  end
end 