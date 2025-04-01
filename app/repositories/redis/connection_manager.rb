require 'redis'
require 'connection_pool'

module RedisSupport
  # Handles Redis connection management and low-level Redis operations
  class ConnectionManager
    DEFAULT_POOL_SIZE = 5
    DEFAULT_TIMEOUT = 5.0

    attr_accessor :errors

    def initialize(redis = nil, pool_size: DEFAULT_POOL_SIZE)
      @redis = redis || create_redis_pool(pool_size)
      @errors = 0
    end

    def with_redis(&)
      if @redis.is_a?(ConnectionPool)
        @redis.with(&)
      else
        yield @redis
      end
    end

    def with_error_handling(return_on_error: nil)
      yield
    rescue Redis::BaseError, Redis::CannotConnectError, ConnectionPool::TimeoutError => e
      @errors += 1
      log_redis_error(e)
      return_on_error
    end

    def connected?
      with_error_handling(return_on_error: false) do
        with_redis { |redis| redis.ping == "PONG" }
      end
    end

    private

    def log_redis_error(error)
      Rails.logger.error("Redis error: #{error.class} - #{error.message}")
      Rails.logger.error(error.backtrace.join("\n")) if Rails.env.development?
    end

    def create_redis_pool(pool_size)
      redis_config = Rails.application.config.redis || {}

      ConnectionPool.new(size: pool_size, timeout: DEFAULT_TIMEOUT) do
        ::Redis.new(
          host: redis_config[:host] || ENV['REDIS_HOST'] || 'localhost',
          port: redis_config[:port] || ENV['REDIS_PORT'] || 6379,
          db: redis_config[:db] || ENV['REDIS_DB'] || 0,
          password: redis_config[:password] || ENV.fetch('REDIS_PASSWORD', nil),
          timeout: DEFAULT_TIMEOUT,
          reconnect_attempts: 3
        )
      end
    end
  end
end