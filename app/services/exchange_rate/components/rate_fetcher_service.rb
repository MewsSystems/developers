# Service responsible for fetching rates, either from cache or provider
class RateFetcherService
  def initialize(provider, repository, cache_strategy, provider_name)
    @provider = provider
    @repository = repository
    @cache_strategy = cache_strategy
    @provider_name = provider_name
  end

  def fetch_rates(force_refresh = false)
    # Get appropriate date from cache strategy
    fetch_date = @cache_strategy.determine_fetch_date

    # Try to get rates from cache
    rates = @cache_strategy.get_cached_rates(fetch_date, force_refresh)

    # If no cached rates or cache is stale, fetch from provider
    unless rates
      begin
        # Fetch fresh rates from provider
        rates = @provider.fetch_rates

        # Save to repository
        @repository.save_for(fetch_date, rates)
      rescue => e
        # Let the cache strategy handle the error
        # It might provide stale data or re-raise the error
        rates = @cache_strategy.handle_fetch_error(e, fetch_date)
      end
    end

    rates
  end
end 