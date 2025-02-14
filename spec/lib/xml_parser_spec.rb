require 'rspec'
require_relative '../../lib/xml_parser'

describe XMLParser do
  let(:xml_body) do
    <<-XML
      <?xml version="1.0" encoding="UTF-8"?>
      <kurzy banka="CNB" datum="10.02.2025" poradi="28">
          <tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">
              <radek kod="EUR" mena="euro" mnozstvi="1" kurz="25,065" zeme="EMU"/>
              <radek kod="USD" mena="dolar" mnozstvi="1" kurz="24,282" zeme="USA"/>
              <radek kod="GBP" mena="libra" mnozstvi="1" kurz="30,093" zeme="Velká Británie"/>
          </tabulka>
      </kurzy>
    XML
  end

  it 'parses exchange rates correctly from Czech bank XML' do
    parsed_data = described_class.parse_exchange_rates_to_json(
      xml_body,
      xpath: '//radek',
      attributes: { currency_code: 'kod', rate: 'kurz', currency: 'mena' }
    )

    expect(parsed_data).to eq([
      { currency_code: 'EUR', rate: '25,065', currency: 'euro' },
      { currency_code: 'USD', rate: '24,282', currency: 'dolar' },
      { currency_code: 'GBP', rate: '30,093', currency: 'libra' }
    ])
  end
end
