require 'redis'

# Configure Redis settings
#
# Redis will be used to cache exchange rates with persistence
# to make the service production-ready
#
# Configuration options can be overridden with environment variables:
# - REDIS_HOST: Hostname/IP of the Redis server
# - REDIS_PORT: Port of the Redis server
# - REDIS_DB: Redis database number
# - REDIS_PASSWORD: Password for Redis authentication if required
# - REDIS_URL: Full Redis URL (overrides individual settings)

Rails.application.config.after_initialize do
  redis_config = {
    host: ENV['REDIS_HOST'] || 'localhost',
    port: ENV['REDIS_PORT'] || 6379,
    db: ENV['REDIS_DB'] || 0
  }

  # Add password if provided
  redis_config[:password] = ENV['REDIS_PASSWORD'] if ENV['REDIS_PASSWORD'].present?

  # Use Redis URL if provided (overrides other settings)
  if ENV['REDIS_URL'].present?
    redis_config = { url: ENV['REDIS_URL'] }
  end

  # Store the configuration
  Rails.application.config.redis = redis_config

  # Test Redis connection
  begin
    redis = Redis.new(redis_config)
    redis.ping
    Rails.logger.info "Connected to Redis at #{redis_config[:host]}:#{redis_config[:port]}"
  rescue Redis::CannotConnectError => e
    Rails.logger.warn "WARNING: Could not connect to Redis: #{e.message}"
    Rails.logger.warn "The application will function, but exchange rates will not be persisted"
  end
end