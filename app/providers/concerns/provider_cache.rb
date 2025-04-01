module ProviderCache
  extend ActiveSupport::Concern

  # Calculate cache TTL based on provider metadata
  # @param metadata [Hash] Provider metadata
  # @param current_time [Time] Current time (defaults to Time.now)
  # @return [Integer] Cache TTL in seconds
  def cache_ttl(metadata, current_time = Time.zone.now)
    update_frequency = metadata[:update_frequency]

    # Default TTLs by update frequency
    ttl_defaults = Utils::DateTimeHelper.get_default_ttls

    # Use default TTL or calculate based on next publication
    if update_frequency == :daily && metadata[:publication_time]
      Utils::DateTimeHelper.calculate_ttl_until_next_publication(
        update_frequency,
        metadata[:publication_time],
        current_time,
        ttl_defaults[:daily]
      )
    else
      # Use default TTL for this frequency or fall back to hourly (3600)
      ttl_defaults[update_frequency] || 3600
    end
  end

  # Check if cached data is still fresh
  # @param cached_at [Time] When the data was cached
  # @param ttl [Integer] Time to live in seconds
  # @param current_time [Time] Current time
  # @return [Boolean] Whether the cache is still fresh
  def cache_fresh?(cached_at, ttl, current_time = Time.zone.now)
    return false unless cached_at

    elapsed_time = current_time.to_i - cached_at.to_i
    elapsed_time < ttl
  end

  # Helper method to safely execute a block and return a default value if it fails
  # @param default [Object] Default value to return on error
  # @yield Block to execute
  # @return [Object] Result of block or default
  def execute_with_safe_handling(default = [])
    yield
  rescue StandardError => e
    # If the operation fails, log a warning and return the default
    log_message("Operation failed: #{e.message}", :warn, self.class.name) if respond_to?(:log_message)
    default
  end
end