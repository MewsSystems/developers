class HealthService
  def check_health
    status = { status: 'ok', time: Time.now.iso8601 }
    repository_health = RepositoryHealthService.new.check_repository_health
    status.merge!(repository_health)
    status
  end
end

class RepositoryHealthService
  def check_repository_health
    status = {}
    add_repository_health(status)
    status
  end

  private

  def add_repository_health(status)
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
  end
end 