require_relative '../../../errors/exchange_rate_errors'

# Service responsible for currency conversion
class CurrencyConverterService
  def initialize(rate_service)
    @rate_service = rate_service
  end

  # Convert an amount from one currency to another
  # @param amount [Numeric] Amount to convert
  # @param from_currency [String] From currency code
  # @param to_currency [String] To currency code
  # @return [Hash] Hash with :amount, :from, :to, and :rate
  def convert(amount, from_currency, to_currency)
    amount = amount.to_f

    # Validate amount
    if amount <= 0
      raise ExchangeRateErrors::ValidationError.new(
        "Amount must be positive: #{amount}",
        nil, nil, { amount: amount }
      )
    end

    # Get the exchange rate
    rate = @rate_service.get_rate(from_currency, to_currency)

    # Calculate converted amount
    converted_amount = amount * rate.rate

    {
      amount: amount,
      from: from_currency,
      to: to_currency,
      rate: rate.rate,
      converted_amount: converted_amount,
      date: rate.date
    }
  end
end 