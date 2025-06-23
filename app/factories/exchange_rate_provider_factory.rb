# frozen_string_literal: true

require_relative '../providers/czech_exchange_rate_provider'

class ExchangeRateProviderFactory
  PROVIDERS = {
    'CZK' => CzechExchangeRateProvider
  }

  def self.create(source)
    provider_class = PROVIDERS[source]
    raise UnsupportedProviderError, source unless provider_class

    provider_class.new
  end
end
