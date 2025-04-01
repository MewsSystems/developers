module RedisSupport
  # Tracks cache metrics
  class CacheMetrics
    attr_reader :hits, :misses, :errors
    attr_writer :hits, :misses, :errors

    def initialize
      @hits = 0
      @misses = 0
      @errors = 0
    end

    def record_hit
      @hits += 1
    end

    def record_miss
      @misses += 1
    end

    def record_error
      @errors += 1
    end

    def hit_ratio
      total = @hits + @misses
      total > 0 ? (@hits.to_f / total) : 0
    end
  end
end 