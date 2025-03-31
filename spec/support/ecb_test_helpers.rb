# Helper methods for ECB provider tests
module ECBTestHelpers
  def sample_ecb_xml_data
    <<-XML
<?xml version="1.0" encoding="UTF-8"?>
<gesmes:Envelope xmlns:gesmes="http://www.gesmes.org/xml/2002-08-01" xmlns="http://www.ecb.int/vocabulary/2002-08-01/eurofxref">
  <gesmes:subject>Reference rates</gesmes:subject>
  <gesmes:Sender>
    <gesmes:name>European Central Bank</gesmes:name>
  </gesmes:Sender>
  <Cube>
    <Cube time="2023-04-24">
      <Cube currency="USD" rate="1.0832"/>
      <Cube currency="JPY" rate="146.68"/>
      <Cube currency="BGN" rate="1.9558"/>
      <Cube currency="CZK" rate="23.653"/>
      <Cube currency="GBP" rate="0.88640"/>
    </Cube>
  </Cube>
</gesmes:Envelope>
    XML
  end
  
  def create_sample_ecb_rates
    [
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('USD'), rate: 1.0832),
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('JPY'), rate: 146.68),
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('CZK'), rate: 23.653),
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('GBP'), rate: 0.8864)
    ]
  end
end 