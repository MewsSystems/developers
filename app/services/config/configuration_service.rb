require_relative '../utils/logging_helper'

class ConfigurationService
  include LoggingHelper

  # Default configuration options
  DEFAULT_CONFIG = {
    'provider' => 'cnb',
    'provider_config' => {
      'base_url' => 'https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt',
      'base_currency' => 'CZK'
    }
  }.freeze

  def initialize(config_path = nil)
    @config = load_config(config_path)
    override_from_env_vars
  end

  # Get the complete configuration hash
  # @return [Hash] The configuration
  def config
    @config.dup
  end

  # Get the provider type from configuration
  # @return [String] Provider type
  def provider_type
    @config['provider']
  end

  # Get the provider configuration
  # @return [Hash] Provider configuration
  def provider_config
    @config['provider_config'] || {}
  end

  # Get a specific configuration value
  # @param key [String] Configuration key
  # @param default [Object] Default value if key is not found
  # @return [Object] Configuration value
  def get(key, default = nil)
    keys = key.to_s.split('.')
    value = @config

    keys.each do |k|
      value = value.is_a?(Hash) ? value[k] : nil
      break if value.nil?
    end

    value || default
  end

  private

  # Load configuration from a YAML file
  # @param config_path [String] Path to configuration file
  # @return [Hash] Loaded configuration
  def load_config(config_path)
    config = DEFAULT_CONFIG.dup

    if config_path && File.exist?(config_path)
      begin
        yaml_config = YAML.load_file(config_path)
        # Deep merge the configuration
        deep_merge!(config, yaml_config)
      rescue => e
        log_error("Error loading configuration from #{config_path}: #{e.message}")
      end
    end

    config
  end

  # Override configuration with environment variables
  def override_from_env_vars
    # EXCHANGE_RATE_PROVIDER - sets the provider type
    if provider_env = ENV['EXCHANGE_RATE_PROVIDER']
      @config['provider'] = provider_env.downcase
    end

    # EXCHANGE_RATE_BASE_URL - provider-specific base URL
    if base_url = ENV['EXCHANGE_RATE_BASE_URL']
      @config['provider_config'] ||= {}
      @config['provider_config']['base_url'] = base_url
    end

    # EXCHANGE_RATE_BASE_CURRENCY - provider-specific base currency
    if base_currency = ENV['EXCHANGE_RATE_BASE_CURRENCY']
      @config['provider_config'] ||= {}
      @config['provider_config']['base_currency'] = base_currency
    end

    # Additional provider-specific environment variables
    # Format: EXCHANGE_RATE_PROVIDER_PARAM_NAME (e.g., EXCHANGE_RATE_PROVIDER_API_KEY)
    ENV.each do |key, value|
      if key.start_with?('EXCHANGE_RATE_PROVIDER_')
        param_name = key.sub('EXCHANGE_RATE_PROVIDER_', '').downcase
        @config['provider_config'] ||= {}
        @config['provider_config'][param_name] = value
      end
    end
  end

  # Deep merge two hashes
  # @param hash [Hash] Target hash
  # @param other_hash [Hash] Source hash
  # @return [Hash] Merged hash
  def deep_merge!(hash, other_hash)
    other_hash.each do |key, value|
      if value.is_a?(Hash) && hash[key].is_a?(Hash)
        deep_merge!(hash[key], value)
      else
        hash[key] = value
      end
    end
    hash
  end
end