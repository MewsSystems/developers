module DTOs
  class ExchangeRateDTO
    attr_reader :from, :to, :rate, :date

    def initialize(from, to, rate, date = nil)
      @from = from
      @to = to
      @rate = rate
      @date = date
    end

    # Create DTO from domain model
    def self.from_domain(exchange_rate)
      new(
        exchange_rate.from.code,
        exchange_rate.to.code,
        exchange_rate.rate,
        exchange_rate.respond_to?(:date) ? exchange_rate.date : nil
      )
    end

    # Create a collection of DTOs from domain models
    def self.from_domain_collection(exchange_rates)
      exchange_rates.map { |rate| from_domain(rate) }
    end

    def to_h
      {
        from: from,
        to: to,
        rate: rate,
        date: date
      }
    end
  end
end