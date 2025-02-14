# frozen_string_literal: true

# rubocop:disable RSpec/DescribeClass

require 'rspec'
require 'rack/test'
require_relative '../../app'
require_relative '../../lib/http_client'

describe 'GET /api/exchange_rates Integration' do
  include Rack::Test::Methods

  before do
    allow(HTTPClient).to receive(:get).with(
      'https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml',
      'CZK'
    ).and_return(<<-XML)
      <?xml version="1.0" encoding="UTF-8"?>
      <kurzy banka="CNB" datum="10.02.2025" poradi="28">
          <tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">
              <radek kod="EUR" mena="Euro" mnozstvi="1" kurz="25,065" zeme="EMU"/>
              <radek kod="USD" mena="Dólar" mnozstvi="1" kurz="24,282" zeme="USA"/>
              <radek kod="GBP" mena="Libra" mnozstvi="1" kurz="30,093" zeme="Reino Unido"/>
          </tabulka>
      </kurzy>
    XML
  end

  it 'fetches exchange rates from the CNB' do
    get '/api/exchange_rates', source: 'CZK'

    expect(last_response.status).to eq(200)
    parsed_response = JSON.parse(last_response.body)

    expect(parsed_response).to have_key('rates')
    expect(parsed_response['rates']).to have_key('CZK')

    rates = parsed_response['rates']['CZK']

    expect(rates.size).to be >= 3

    expect(rates).to include(
      { 'currency_code' => 'EUR', 'rate' => 25.065, 'currency' => 'Euro' },
      { 'currency_code' => 'USD', 'rate' => 24.282, 'currency' => 'Dólar' },
      { 'currency_code' => 'GBP', 'rate' => 30.093, 'currency' => 'Libra' }
    )
  end
end

# rubocop:enable RSpec/DescribeClass
