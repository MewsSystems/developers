module ExchangeRateResponseFormatter
  extend ActiveSupport::Concern

  private

  # Format an exchange rate for API response
  def format_rate(rate)
    {
      from: rate.from.code,
      to: rate.to.code,
      rate: rate.rate,
      date: rate.date
    }
  end

  # Add warnings about unavailable currencies to response
  def add_warnings_to_response(response)
    unavailable = exchange_rate_service.unavailable_currencies
    return unless unavailable.any?

    response[:warnings] = {
      unavailable_currencies: unavailable.keys,
      available_currencies: exchange_rate_service.provider.supported_currencies - unavailable.keys,
      provider: exchange_rate_service.provider.metadata[:source_name]
    }
  end
end