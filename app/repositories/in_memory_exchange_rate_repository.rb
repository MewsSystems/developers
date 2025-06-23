class InMemoryExchangeRateRepository < ExchangeRateRepository
  def initialize
    super
    @store = {}
  end

  def fetch_for(date, allow_stale: false)
    @store[date]
  end

  def save_for(date, rates)
    @store[date] = rates
    @metadata[date] = { cached_at: Time.now }
    rates
  end
end