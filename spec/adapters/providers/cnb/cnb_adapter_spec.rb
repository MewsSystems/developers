require 'rails_helper'
require_relative '../../../../app/adapters/providers/cnb/cnb_adapter'

RSpec.describe CnbAdapter do
  let(:base_currency) { 'CZK' }
  let(:sample_data) { File.read(File.join(Rails.root, 'spec/fixtures/cnb_sample.txt')) }

  it "parses the CNB TXT data into ExchangeRate objects" do
    rates = CnbAdapter.parse(sample_data, base_currency)
    
    # Check a few currencies from the sample data
    usd_rate = rates.find { |r| r.to.code == 'USD' }
    eur_rate = rates.find { |r| r.to.code == 'EUR' }
    jpy_rate = rates.find { |r| r.to.code == 'JPY' }
    
    expect(usd_rate).not_to be_nil
    expect(usd_rate.from.code).to eq 'CZK'
    expect(usd_rate.to.code).to eq 'USD'
    expect(usd_rate.rate).to be_within(0.0001).of(23.117)

    expect(eur_rate).not_to be_nil
    expect(eur_rate.to.code).to eq 'EUR'
    expect(eur_rate.rate).to be_within(0.0001).of(24.93)

    expect(jpy_rate).not_to be_nil
    expect(jpy_rate.to.code).to eq 'JPY'
    # In the fixture, 100 JPY = 15.376 CZK, so normalized rate is 0.15376
    expect(jpy_rate.rate).to be_within(0.0001).of(0.15376)
  end

  it "correctly handles different amount values" do
    rates = CnbAdapter.parse(sample_data, base_currency)
    
    # Check currencies with amount > 1
    huf_rate = rates.find { |r| r.to.code == 'HUF' }
    idr_rate = rates.find { |r| r.to.code == 'IDR' }
    
    expect(huf_rate).not_to be_nil
    # 100 HUF = 6.152 CZK -> 1 HUF = 0.06152 CZK
    expect(huf_rate.rate).to be_within(0.0001).of(0.06152)
    
    expect(idr_rate).not_to be_nil
    # 1000 IDR = 1.459 CZK -> 1 IDR = 0.001459 CZK
    expect(idr_rate.rate).to be_within(0.00001).of(0.001459)
  end

  it "reuses Currency objects for identical codes" do
    rates = CnbAdapter.parse(sample_data, base_currency)
    
    # All rates should have the same 'from' currency instance
    from_currencies = rates.map(&:from).uniq
    expect(from_currencies.size).to eq 1
    expect(from_currencies.first.code).to eq 'CZK'
  end

  it "raises ParseError for empty data" do
    expect { CnbAdapter.parse("", base_currency) }.to raise_error(CnbAdapter::ParseError)
  end

  it "raises ParseError for invalid header format" do
    bad_data = "26.03.2025 #60\nInvalid Header\nAustralia|dollar|1|AUD|14.602"
    expect { CnbAdapter.parse(bad_data, base_currency) }.to raise_error(CnbAdapter::ParseError)
  end

  it "raises ParseError for malformed line" do
    bad_data = "26.03.2025 #60\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD" # missing rate
    expect { CnbAdapter.parse(bad_data, base_currency) }.to raise_error(CnbAdapter::ParseError)
  end

  it "raises ParseError for invalid amount" do
    bad_data = "26.03.2025 #60\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|0|AUD|14.602" # amount is 0
    expect { CnbAdapter.parse(bad_data, base_currency) }.to raise_error(CnbAdapter::ParseError)
  end

  it "raises ParseError for invalid rate" do
    bad_data = "26.03.2025 #60\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|abc" # non-numeric rate
    expect { CnbAdapter.parse(bad_data, base_currency) }.to raise_error(CnbAdapter::ParseError)
  end
end 