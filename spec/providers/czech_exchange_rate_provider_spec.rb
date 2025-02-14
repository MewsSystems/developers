# frozen_string_literal: true

require_relative '../../app/providers/exchange_rate_provider'
require_relative '../../app/providers/czech_exchange_rate_provider'
require_relative '../../lib/http_client'

describe CzechExchangeRateProvider do
  let(:provider) { described_class.new }
  let(:api_url) { 'https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml' }
  let(:source_provider) { 'CZK' }

  before do
    allow(HTTPClient).to receive(:get).with(api_url, source_provider).and_return(<<-XML)
      <?xml version="1.0" encoding="UTF-8"?>
      <kurzy banka="CNB" datum="10.02.2025" poradi="28">
          <tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">
              <radek kod="EUR" mena="euro" mnozstvi="1" kurz="25,065" zeme="EMU"/>
              <radek kod="USD" mena="dolar" mnozstvi="1" kurz="24,282" zeme="USA"/>
              <radek kod="GBP" mena="libra" mnozstvi="1" kurz="30,093" zeme="Velká Británie"/>
          </tabulka>
      </kurzy>
    XML

    allow(XMLParser).to receive(:parse_exchange_rates_to_json).with(
      instance_of(String),
      xpath: '//radek',
      attributes: { currency_code: 'kod', rate: 'kurz', currency: 'mena' }
    ).and_return([
      { currency_code: 'EUR', rate: '25,065', currency: 'Euro' },
      { currency_code: 'USD', rate: '24,282', currency: 'Dolar' },
      { currency_code: 'GBP', rate: '30,093', currency: 'Libra' }
    ])
  end

  it 'fetches and parses exchange rates correctly' do
    rates = provider.request_exchange_rates

    expect(HTTPClient).to have_received(:get).with(api_url, source_provider)
    expect(XMLParser).to have_received(:parse_exchange_rates_to_json).with(
      instance_of(String),
      xpath: '//radek',
      attributes: { currency_code: 'kod', rate: 'kurz', currency: 'mena' }
    )

    expect(rates).to eq({
      'CZK' => [
        { currency_code: 'EUR', rate: 25.065, currency: 'Euro' },
        { currency_code: 'USD', rate: 24.282, currency: 'Dolar' },
        { currency_code: 'GBP', rate: 30.093, currency: 'Libra' }
      ]
    })
  end

  context 'when the API fails multiple times' do
    before do
      allow(HTTPClient).to receive(:get).with(api_url, source_provider)
                                        .and_raise(ProviderNotAvailableError, 'CZK')
    end

    it 'raises ProviderNotAvailableError after 3 failed attempts' do
      expect { provider.request_exchange_rates }
        .to raise_error(ProviderNotAvailableError, 'API from provider CZK is not available')
    end
  end
end
