require 'rails_helper'

RSpec.describe ExchangeRate do
  let(:czk) { Currency.new('CZK', 'Czech Koruna') }
  let(:usd) { Currency.new('USD', 'US Dollar') }

  it "initializes with Currency objects and rate" do
    rate = ExchangeRate.new(from: czk, to: usd, rate: 22.5)
    expect(rate.from).to eq czk
    expect(rate.to).to eq usd
    expect(rate.rate).to eq 22.5
  end

  it "can initialize with currency codes (strings) and converts to Currency objects" do
    rate = ExchangeRate.new(from: 'CZK', to: 'EUR', rate: 25.0)
    expect(rate.from).to be_a(Currency)
    expect(rate.from.code).to eq 'CZK'
    expect(rate.to.code).to eq 'EUR'
    expect(rate.rate).to eq 25.0
  end

  it "raises an error for non-positive rates" do
    expect { ExchangeRate.new(from: czk, to: usd, rate: 0) }.to raise_error(ArgumentError)
    expect { ExchangeRate.new(from: czk, to: usd, rate: -5) }.to raise_error(ArgumentError)
  end

  it "enforces that inverse rates are not provided" do
    rate = ExchangeRate.new(from: czk, to: usd, rate: 22.0)
    expect { rate.inverse }.to raise_error(StandardError, /not allowed/)
  end

  it "converts to hash correctly for JSON serialization" do
    rate = ExchangeRate.new(from: czk, to: usd, rate: 22.0)
    expect(rate.to_h).to eq({ from: 'CZK', to: 'USD', rate: 22.0 })
  end
end