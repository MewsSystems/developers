module ExchangeRateTestHelper
  # Sample data for testing
  SAMPLE_RATES = {
    'USD' => 21.5,
    'EUR' => 25.8,
    'GBP' => 30.2,
    'JPY' => 0.19,
    'AUD' => 15.7
  }.freeze

  # Create sample exchange rates for the given base currency
  # @param base_currency [String] Base currency code
  # @param date [Date] Date for the rates
  # @return [Array<ExchangeRate>] Sample exchange rates
  def create_sample_rates(base_currency = 'CZK', date = Date.today)
    SAMPLE_RATES.map do |code, rate|
      ExchangeRate.new(
        from: Currency.new(base_currency),
        to: Currency.new(code),
        rate: rate,
        date: date
      )
    end
  end

  # Create sample ECB exchange rates (inverse of normal rates)
  # @param base_currency [String] Base currency code (usually EUR)
  # @param date [Date] Date for the rates
  # @return [Array<ExchangeRate>] Sample exchange rates
  def create_sample_ecb_rates(base_currency = 'EUR', date = Date.today)
    {
      'USD' => 1.0832,
      'CZK' => 25.388,
      'GBP' => 0.8581,
      'JPY' => 164.51,
      'AUD' => 1.6325
    }.map do |code, rate|
      ExchangeRate.new(
        from: Currency.new(base_currency),
        to: Currency.new(code),
        rate: rate,
        date: date
      )
    end
  end

  # Sample CNB TXT format data
  def sample_cnb_txt_data(date = Date.today)
    formatted_date = date.strftime("%d.%m.%Y")
    <<~TXT
      #{formatted_date} #39
      Country|Currency|Amount|Code|Rate
      Australia|dollar|1|AUD|15.700
      Euro area|euro|1|EUR|25.800
      Great Britain|pound|1|GBP|30.200
      Japan|yen|100|JPY|19.000
      USA|dollar|1|USD|21.500
    TXT
  end

  # Sample ECB XML format data
  def sample_ecb_xml_data(date = Date.today)
    formatted_date = date.strftime("%Y-%m-%d")
    <<~XML
      <?xml version="1.0" encoding="UTF-8"?>
      <gesmes:Envelope xmlns:gesmes="http://www.gesmes.org/xml/2002-08-01" xmlns="http://www.ecb.int/vocabulary/2002-08-01/eurofxref">
        <gesmes:subject>Reference rates</gesmes:subject>
        <gesmes:Sender>
          <gesmes:name>European Central Bank</gesmes:name>
        </gesmes:Sender>
        <Cube>
          <Cube time="#{formatted_date}">
            <Cube currency="USD" rate="1.0832"/>
            <Cube currency="JPY" rate="164.51"/>
            <Cube currency="CZK" rate="25.388"/>
            <Cube currency="GBP" rate="0.8581"/>
            <Cube currency="AUD" rate="1.6325"/>
          </Cube>
        </Cube>
      </gesmes:Envelope>
    XML
  end

  # Sample provider JSON format data
  def sample_json_data(base_currency = 'CZK', date = Date.today)
    formatted_date = date.strftime("%Y-%m-%d")
    rates = base_currency == 'CZK' ? SAMPLE_RATES : create_sample_ecb_rates.map { |r| [r.to.code, r.rate] }.to_h

    {
      date: formatted_date,
      base: base_currency,
      rates: rates
    }.to_json
  end

  # Create HTTP response mock
  def mock_http_response(body, content_type, status = 200)
    {
      data: body,
      content_type: content_type,
      status: status,
      headers: { 'Content-Type' => content_type },
      last_modified: Time.now.httpdate
    }
  end

  # Sample CNB text data
  def sample_cnb_text_data(date = Date.today)
    formatted_date = date.strftime("%d.%m.%Y")
    "#{formatted_date} #1\n" \
      "Country|Currency|Amount|Code|Rate\n" \
      "Australia|dollar|1|AUD|15.376\n" \
      "Canada|dollar|1|CAD|18.798\n" \
      "Euro|euro|1|EUR|26.760\n" \
      "Great Britain|pound|1|GBP|31.566\n" \
      "Japan|yen|100|JPY|19.379\n" \
      "USA|dollar|1|USD|24.975"
  end

  # Sample CNB json data
  def sample_cnb_json_data(base_currency = 'CZK', date = Date.today)
    formatted_date = date.strftime("%Y-%m-%d")
    {
      date: formatted_date,
      source: "CNB",
      rates: [
        { code: "AUD", currency: "dollar", amount: 1, rate: 15.376 },
        { code: "CAD", currency: "dollar", amount: 1, rate: 18.798 },
        { code: "EUR", currency: "euro", amount: 1, rate: 26.760 },
        { code: "GBP", currency: "pound", amount: 1, rate: 31.566 },
        { code: "JPY", currency: "yen", amount: 100, rate: 19.379 },
        { code: "USD", currency: "dollar", amount: 1, rate: 24.975 }
      ]
    }.to_json
  end
end