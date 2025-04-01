class HealthController < ApplicationController
  # Health check endpoint
  # Used for container health checks and monitoring
  def index
    status = { status: 'ok', time: Time.now.iso8601 }

    # Get Redis repository for metrics
    begin
      repository = Rails.application.services&.get(:repository)

      # Add repository metrics if available (for Redis repository)
      if repository.respond_to?(:health_metrics)
        metrics = repository.health_metrics
        status[:redis] = metrics[:redis_connected] ? 'connected' : 'disconnected'
        status[:cache] = {
          size: metrics[:cache_size],
          hit_ratio: metrics[:cache_hit_ratio].round(2),
          hits: metrics[:cache_hits],
          misses: metrics[:cache_misses]
        }
        status[:errors] = {
          count: metrics[:error_count],
          fallback_active: metrics[:fallback_size] > 0
        }
      else
        # Fallback to basic Redis check
        redis_config = Rails.application.config.redis || {}
        redis = Redis.new(redis_config)
        redis.ping  # This will raise an exception if Redis is not available
        status[:redis] = 'connected'
      end
    rescue => e
      status[:redis] = 'disconnected'
      status[:error] = e.message if Rails.env.development?
    end

    render json: status
  end

  # Debug endpoint for Redis testing (development only)
  def redis_debug
    return head :forbidden unless Rails.env.development?

    repository = Rails.application.services&.get(:repository)

    # Test Redis operations
    test_date = Date.today
    test_rate = ExchangeRate.new(from: 'USD', to: 'EUR', rate: 0.93, date: test_date)

    # Save a test rate
    repository.save_for(test_date, [test_rate])

    # Fetch the rate back
    fetched = repository.fetch_for(test_date)

    # Test cache miss by clearing and fetching again
    repository.clear(test_date)
    repository.fetch_for(test_date)

    # Get all metrics
    metrics = repository.health_metrics

    render json: {
      repository_class: repository.class.name,
      metrics: metrics,
      test_results: {
        save_successful: !fetched.nil?,
        fetch_count: fetched&.size || 0,
        rates: fetched&.map { |r| { from: r.from.code, to: r.to.code, rate: r.rate } }
      }
    }
  end
end