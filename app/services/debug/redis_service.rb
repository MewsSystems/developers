module Debug
  class RedisService
    def test_operations
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

      {
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
end 