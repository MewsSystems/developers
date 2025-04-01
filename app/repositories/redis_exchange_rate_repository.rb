require 'redis'
require 'json'
require 'msgpack'
require 'connection_pool'

class RedisExchangeRateRepository < ExchangeRateRepository
  # Default TTL for cached rates (1 day in seconds)
  DEFAULT_TTL = 86400
  DEFAULT_POOL_SIZE = 5
  DEFAULT_TIMEOUT = 5.0

  # Stats for monitoring
  attr_reader :cache_hits, :cache_misses, :errors

  # @param redis [Redis, ConnectionPool] Redis connection instance or pool
  # @param ttl [Integer] Default TTL for cached rates in seconds
  # @param prefix [String] Key prefix for Redis keys
  # @param pool_size [Integer] Size of the connection pool
  def initialize(redis = nil, ttl: DEFAULT_TTL, prefix: 'exchange_rates', pool_size: DEFAULT_POOL_SIZE)
    super()
    @redis = redis || create_redis_pool(pool_size)
    @ttl = ttl
    @prefix = prefix
    @fallback_cache = {}
    @fallback_metadata = {}
    @cache_hits = 0
    @cache_misses = 0
    @errors = 0
  end

  # Fetch exchange rates for a specific date
  # @param date [Date] The date for which to fetch rates
  # @param allow_stale [Boolean] Whether to allow stale data
  # @return [Array<ExchangeRate>, nil] Array of exchange rates or nil if not found
  def fetch_for(date, allow_stale: false)
    result = nil
    with_redis_error_handling do
      data = with_redis { |redis| redis.get(key_for(date)) }
      if data
        @cache_hits += 1
        result = deserialize_rates(data)
        # Store in fallback cache for resilience
        @fallback_cache[date] = result
        return result
      end
    end

    # Fallback to in-memory cache if Redis failed or data not found
    if @fallback_cache[date]
      @cache_hits += 1
      return @fallback_cache[date]
    end

    @cache_misses += 1
    nil
  end

  # Save exchange rates for a specific date
  # @param date [Date] The date for which to save rates
  # @param rates [Array<ExchangeRate>] The exchange rates to save
  # @return [Array<ExchangeRate>] The saved exchange rates
  def save_for(date, rates)
    serialized_data = serialize_rates(rates)
    metadata = { cached_at: Time.now.to_s }

    with_redis_error_handling do
      with_redis do |redis|
        # Store rates data
        redis.setex(key_for(date), @ttl, serialized_data)

        # Store metadata
        redis.setex(metadata_key_for(date), @ttl, metadata.to_json)
      end
    end

    # Always update fallback cache
    @fallback_cache[date] = rates
    @fallback_metadata[date] = metadata

    rates
  end

  # Get caching metadata for a specific date
  # @param date [Date] The date to get metadata for
  # @return [Hash, nil] Metadata hash or nil if not available
  def metadata_for(date)
    with_redis_error_handling do
      data = with_redis { |redis| redis.get(metadata_key_for(date)) }
      return JSON.parse(data, symbolize_names: true) if data
    end

    # Fallback to in-memory cache
    @fallback_metadata[date]
  end

  # Get the time when data for a specific date was last cached
  # @param date [Date] The date to check
  # @return [Time, nil] Cache time or nil if not cached
  def cache_time_for(date)
    meta = metadata_for(date)
    meta && Time.parse(meta[:cached_at])
  end

  # Clear cached data for a specific date
  # @param date [Date] The date to clear
  def clear(date)
    with_redis_error_handling do
      with_redis { |redis| redis.del(key_for(date), metadata_key_for(date)) }
    end

    # Always clear fallback cache
    @fallback_cache.delete(date)
    @fallback_metadata.delete(date)
  end

  # Clear all cached data
  def clear_all
    with_redis_error_handling do
      with_redis do |redis|
        keys = redis.keys("#{@prefix}:*")
        redis.del(*keys) unless keys.empty?
      end
    end

    # Always clear fallback cache
    @fallback_cache.clear
    @fallback_metadata.clear
  end

  # Check if data exists for a specific date
  # @param date [Date] The date to check
  # @return [Boolean] Whether data exists for the date
  def has_data_for?(date)
    with_redis_error_handling do
      return with_redis { |redis| redis.exists?(key_for(date)) }
    end

    # Fallback to in-memory cache if Redis failed
    @fallback_cache.key?(date)
  end

  # Get all cached dates
  # @return [Array<Date>] Array of dates for which data is cached
  def cached_dates
    with_redis_error_handling do
      keys = with_redis { |redis| redis.keys("#{@prefix}:rates:*") }
      return keys.map { |key| extract_date_from_key(key) }.compact
    end

    # Fallback to in-memory cache if Redis failed
    @fallback_cache.keys
  end

  # Health metrics for monitoring
  # @return [Hash] Hash of health metrics
  def health_metrics
    cache_total = @cache_hits + @cache_misses
    hit_ratio = cache_total > 0 ? (@cache_hits.to_f / cache_total) : 0

    {
      redis_connected: redis_connected?,
      cache_size: cached_dates.size,
      cache_hit_ratio: hit_ratio,
      cache_hits: @cache_hits,
      cache_misses: @cache_misses,
      error_count: @errors,
      fallback_size: @fallback_cache.size
    }
  end

  # Check if Redis is connected
  # @return [Boolean] Whether Redis is connected
  def redis_connected?
    with_redis_error_handling(return_on_error: false) do
      with_redis { |redis| redis.ping == "PONG" }
    end
  end

  private

  # Execute a block with Redis error handling
  # @param return_on_error [Boolean] What to return if an error occurs
  # @yield Block to execute
  # @return [Object, nil] Result of the block or nil if an error occurred
  def with_redis_error_handling(return_on_error: nil)
    yield
  rescue Redis::BaseError, Redis::CannotConnectError, ConnectionPool::TimeoutError => e
    @errors += 1
    log_redis_error(e)
    return_on_error
  end

  # Log Redis errors with context
  # @param error [Exception] Error to log
  def log_redis_error(error)
    Rails.logger.error("Redis error in #{self.class.name}: #{error.class} - #{error.message}")
    Rails.logger.error(error.backtrace.join("\n")) if Rails.env.development?
  end

  # Execute a block with a Redis connection
  # @yield [Redis] Redis connection
  # @return [Object] Result of the block
  def with_redis
    if @redis.is_a?(ConnectionPool)
      @redis.with { |conn| yield conn }
    else
      yield @redis
    end
  end

  # Create a new Redis client pool
  # @param pool_size [Integer] Size of the connection pool
  # @return [ConnectionPool] Redis connection pool
  def create_redis_pool(pool_size)
    redis_config = Rails.application.config.redis || {}

    ConnectionPool.new(size: pool_size, timeout: DEFAULT_TIMEOUT) do
      Redis.new(
        host: redis_config[:host] || ENV['REDIS_HOST'] || 'localhost',
        port: redis_config[:port] || ENV['REDIS_PORT'] || 6379,
        db: redis_config[:db] || ENV['REDIS_DB'] || 0,
        password: redis_config[:password] || ENV['REDIS_PASSWORD'],
        timeout: DEFAULT_TIMEOUT,
        reconnect_attempts: 3
      )
    end
  end

  # Generate a Redis key for a date
  # @param date [Date] The date
  # @return [String] Redis key
  def key_for(date)
    "#{@prefix}:rates:#{date.to_s}"
  end

  # Generate a Redis key for metadata
  # @param date [Date] The date
  # @return [String] Redis metadata key
  def metadata_key_for(date)
    "#{@prefix}:metadata:#{date.to_s}"
  end

  # Extract date from a Redis key
  # @param key [String] Redis key
  # @return [Date, nil] Extracted date or nil
  def extract_date_from_key(key)
    date_str = key.gsub("#{@prefix}:rates:", '')
    Date.parse(date_str) rescue nil
  end

  # Serialize exchange rates for storage
  # @param rates [Array<ExchangeRate>] Exchange rates
  # @return [String] Serialized data
  def serialize_rates(rates)
    rates_data = rates.map do |rate|
      {
        from: rate.from.code,
        to: rate.to.code,
        rate: rate.rate,
        date: rate.date.to_s
      }
    end

    MessagePack.pack(rates_data)
  end

  # Deserialize exchange rates from storage
  # @param data [String] Serialized data
  # @return [Array<ExchangeRate>] Exchange rates
  def deserialize_rates(data)
    # Try MessagePack first, fall back to JSON for backward compatibility
    begin
      rates_data = MessagePack.unpack(data, symbolize_keys: true)
    rescue MessagePack::UnpackError
      # Attempt to parse as JSON (for backward compatibility)
      rates_data = JSON.parse(data, symbolize_names: true)
    end

    rates_data.map do |rate_data|
      ExchangeRate.new(
        from: rate_data[:from],
        to: rate_data[:to],
        rate: rate_data[:rate],
        date: Date.parse(rate_data[:date])
      )
    end
  end
end