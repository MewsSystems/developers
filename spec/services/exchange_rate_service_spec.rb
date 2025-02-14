# frozen_string_literal: true

require_relative '../../app/providers/exchange_rate_provider'
require_relative '../../app/services/exchange_rate_service'
require_relative '../../app/factories/exchange_rate_provider_factory'
require_relative '../../app/errors/unsupported_provider_error'

describe ExchangeRateService do
  let(:provider_instance) { instance_double(CzechExchangeRateProvider) }

  before do
    allow(ExchangeRateProviderFactory).to receive(:create).with('CZK').and_return(provider_instance)
  end

  context 'when no currency filter is provided' do
    it 'returns exchange rates from the correct provider' do
      allow(provider_instance).to receive(:request_exchange_rates).and_return({
        'CZK' => [
          { "currency_code": 'EUR', "rate": 25.065, "currency": 'Euro' },
          { "currency_code": 'USD', "rate": 24.282, "currency": 'Dolar' },
          { "currency_code": 'GBP', "rate": 30.093, "currency": 'Libra' }
        ]
      })

      rates = described_class.get_rates('CZK')

      expect(rates).to eq({
        'CZK' => [
          { "currency_code": 'EUR', "rate": 25.065, "currency": 'Euro' },
          { "currency_code": 'USD', "rate": 24.282, "currency": 'Dolar' },
          { "currency_code": 'GBP', "rate": 30.093, "currency": 'Libra' }
        ]
      })
    end
  end

  context 'when a currency filter is provided' do
    it 'returns only the requested exchange rates' do
      allow(provider_instance).to receive(:request_exchange_rates).and_return({
        'CZK' => [
          { "currency_code": 'EUR', "rate": 25.065, "currency": 'euro' },
          { "currency_code": 'USD', "rate": 24.282, "currency": 'dolar' },
          { "currency_code": 'GBP', "rate": 30.093, "currency": 'libra' },
          { "currency_code": 'JPY', "rate": 15.978, "currency": 'jen' }
        ]
      })

      rates = described_class.get_rates('CZK', ['EUR', 'USD'])

      expect(rates).to eq({
        'CZK' => [
          { "currency_code": 'EUR', "rate": 25.065, "currency": 'euro' },
          { "currency_code": 'USD', "rate": 24.282, "currency": 'dolar' }
        ]
      })
    end
  end
end
