# frozen_string_literal: true

require_relative '../factories/exchange_rate_provider_factory'

class ExchangeRateService
  def self.get_rates(source, currencies = [])
    provider = ExchangeRateProviderFactory.create(source)
    rates = provider.request_exchange_rates

    return rates if currencies.empty?

    rates_filtered = rates[source].select { |rate| currencies.include?(rate[:currency_code]) }

    { source => rates_filtered }
  end
end
