# Service responsible for exchange rate calculations
class RateCalculatorService
  def initialize(provider)
    @provider = provider
  end

  # Calculate exchange rate using various strategies
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Exchange rate or nil if not possible
  def calculate_rate(rates, from_currency, to_currency)
    find_direct_rate(rates, from_currency, to_currency) ||
    calculate_inverse_rate(rates, from_currency, to_currency) ||
    calculate_cross_rate(rates, from_currency, to_currency)
  end

  private

  # Find a direct exchange rate between two currencies
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Direct exchange rate or nil if not found
  def find_direct_rate(rates, from_currency, to_currency)
    rates.find { |rate|
      rate.from.code == from_currency && rate.to.code == to_currency
    }
  end

  # Calculate inverse exchange rate between two currencies
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Inverse exchange rate or nil if not possible
  def calculate_inverse_rate(rates, from_currency, to_currency)
    inverse_rate = rates.find { |rate|
      rate.from.code == to_currency && rate.to.code == from_currency
    }

    return nil unless inverse_rate

    # Return the inverse of the rate
    ExchangeRate.new(
      from: inverse_rate.to,
      to: inverse_rate.from,
      rate: 1.0 / inverse_rate.rate,
      date: inverse_rate.date
    )
  end

  # Calculate cross exchange rate via the base currency
  # @param rates [Array<ExchangeRate>] Available rates
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [ExchangeRate, nil] Cross exchange rate or nil if not possible
  def calculate_cross_rate(rates, from_currency, to_currency)
    base_currency = @provider.metadata[:base_currency]

    # Find rates to convert via base currency
    from_to_base = rates.find { |rate|
      rate.from.code == base_currency && rate.to.code == from_currency
    }

    base_to_to = rates.find { |rate|
      rate.from.code == base_currency && rate.to.code == to_currency
    }

    return nil unless from_to_base && base_to_to

    # Calculate cross rate
    cross_rate = base_to_to.rate / from_to_base.rate

    ExchangeRate.new(
      from: Currency.new(from_currency),
      to: Currency.new(to_currency),
      rate: cross_rate,
      date: base_to_to.date
    )
  end
end 