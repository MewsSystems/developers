require 'redis'
require 'json'
require 'msgpack'
require 'connection_pool'
require_relative 'redis/connection_manager'
require_relative 'redis/exchange_rate_serializer'
require_relative 'redis/cache_metrics'

class RedisExchangeRateRepository < ExchangeRateRepository
  # Default TTL for cached rates (1 day in seconds)
  DEFAULT_TTL = 86400

  # Delegating accessors for test compatibility
  def cache_hits
    @metrics.hits
  end

  def cache_misses
    @metrics.misses
  end

  delegate :errors, to: :@connection

  # For testing: allow setting metrics directly
  def instance_variable_set(name, value)
    case name
    when :@cache_hits
      @metrics.hits = value
    when :@cache_misses
      @metrics.misses = value
    when :@errors
      @connection.errors = value
    else
      super
    end
  end

  def initialize(redis = nil, ttl: DEFAULT_TTL, prefix: 'exchange_rates',
                 pool_size: RedisSupport::ConnectionManager::DEFAULT_POOL_SIZE)
    super()
    @connection = RedisSupport::ConnectionManager.new(redis, pool_size: pool_size)
    @serializer = RedisSupport::ExchangeRateSerializer.new
    @metrics = RedisSupport::CacheMetrics.new
    @ttl = ttl
    @prefix = prefix
    @fallback_cache = {}
    @fallback_metadata = {}
  end

  def fetch_for(date, allow_stale: false)
    result = nil
    @connection.with_error_handling do
      data = @connection.with_redis { |redis| redis.get(key_for(date)) }
      if data
        @metrics.record_hit
        result = @serializer.deserialize(data)
        @fallback_cache[date] = result
        return result
      end
    end

    if @fallback_cache[date]
      @metrics.record_hit
      return @fallback_cache[date]
    end

    @metrics.record_miss
    nil
  end

  def save_for(date, rates)
    serialized_data = @serializer.serialize(rates)
    metadata = { cached_at: Time.now.to_s }

    @connection.with_error_handling do
      @connection.with_redis do |redis|
        redis.setex(key_for(date), @ttl, serialized_data)
        redis.setex(metadata_key_for(date), @ttl, metadata.to_json)
      end
    end

    @fallback_cache[date] = rates
    @fallback_metadata[date] = metadata

    rates
  end

  def metadata_for(date)
    @connection.with_error_handling do
      data = @connection.with_redis { |redis| redis.get(metadata_key_for(date)) }
      return JSON.parse(data, symbolize_names: true) if data
    end

    @fallback_metadata[date]
  end

  def cache_time_for(date)
    meta = metadata_for(date)
    meta && Time.parse(meta[:cached_at])
  end

  def clear(date)
    @connection.with_error_handling do
      @connection.with_redis { |redis| redis.del(key_for(date), metadata_key_for(date)) }
    end

    @fallback_cache.delete(date)
    @fallback_metadata.delete(date)
  end

  def clear_all
    @connection.with_error_handling do
      @connection.with_redis do |redis|
        keys = redis.keys("#{@prefix}:*")
        redis.del(*keys) unless keys.empty?
      end
    end

    @fallback_cache.clear
    @fallback_metadata.clear
  end

  def has_data_for?(date)
    @connection.with_error_handling do
      return @connection.with_redis { |redis| redis.exists?(key_for(date)) }
    end

    @fallback_cache.key?(date)
  end

  def cached_dates
    @connection.with_error_handling do
      keys = @connection.with_redis { |redis| redis.keys("#{@prefix}:rates:*") }
      return keys.map { |key| extract_date_from_key(key) }.compact
    end

    @fallback_cache.keys
  end

  def health_metrics
    {
      redis_connected: redis_connected?,
      cache_size: cached_dates.size,
      cache_hit_ratio: @metrics.hit_ratio,
      cache_hits: @metrics.hits,
      cache_misses: @metrics.misses,
      error_count: @connection.errors,
      fallback_size: @fallback_cache.size
    }
  end

  def redis_connected?
    @connection.connected?
  end

  private

  def key_for(date)
    "#{@prefix}:rates:#{date.to_s}"
  end

  def metadata_key_for(date)
    "#{@prefix}:metadata:#{date.to_s}"
  end

  def extract_date_from_key(key)
    date_str = key.gsub("#{@prefix}:rates:", '')
    Date.parse(date_str) rescue nil
  end
end