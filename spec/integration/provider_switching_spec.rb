require 'spec_helper'

# This integration test verifies that the system works correctly
# with different exchange rate providers
RSpec.describe "Provider switching" do
  let(:cnb_rates) {
    [
      ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('USD'), rate: 23.117),
      ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('EUR'), rate: 24.930),
      ExchangeRate.new(from: Currency.new('CZK'), to: Currency.new('GBP'), rate: 29.178)
    ]
  }
  
  let(:ecb_rates) {
    [
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('USD'), rate: 1.0832),
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('CZK'), rate: 25.388),
      ExchangeRate.new(from: Currency.new('EUR'), to: Currency.new('GBP'), rate: 0.8581)
    ]
  }
  
  before do
    # Mock the providers to return our sample rates
    allow_any_instance_of(CNBProvider).to receive(:fetch_rates).and_return(cnb_rates)
    allow_any_instance_of(ECBProvider).to receive(:fetch_rates).and_return(ecb_rates)
    
    # Mock the provider metadata
    allow_any_instance_of(CNBProvider).to receive(:metadata).and_return({
      source_name: 'Czech National Bank (CNB)',
      base_currency: 'CZK',
      supported_currencies: ['USD', 'EUR', 'GBP'],
      update_frequency: :daily
    })
    
    allow_any_instance_of(ECBProvider).to receive(:metadata).and_return({
      source_name: 'European Central Bank (ECB)',
      base_currency: 'EUR',
      supported_currencies: ['USD', 'CZK', 'GBP'],
      update_frequency: :daily
    })
  end
  
  it "works with the CNB provider" do
    # Set to CNB provider
    ExchangeRateTestConfig.provider_type = 'cnb'
    
    # Create service
    service = ExchangeRateTestConfig.create_service
    
    # Get rates
    rates = service.get_rates
    
    # Verify CNB-specific behavior
    expect(rates).to be_an(Array)
    expect(rates.size).to eq(3)
    expect(rates.first.from.code).to eq('CZK')
    
    # Verify CNB has the currencies we expect
    currencies = rates.map { |rate| rate.to.code }
    expect(currencies).to include('USD', 'EUR', 'GBP')
  end
  
  it "works with the ECB provider" do
    # Set to ECB provider
    ExchangeRateTestConfig.provider_type = 'ecb'
    
    # Create service
    service = ExchangeRateTestConfig.create_service
    
    # Get rates
    rates = service.get_rates
    
    # Verify ECB-specific behavior
    expect(rates).to be_an(Array)
    expect(rates.size).to eq(3)
    expect(rates.first.from.code).to eq('EUR')
    
    # Verify ECB has the currencies we expect
    currencies = rates.map { |rate| rate.to.code }
    expect(currencies).to include('USD', 'CZK', 'GBP')
  end
  
  it "can convert currency regardless of provider" do
    # Test with CNB provider
    ExchangeRateTestConfig.provider_type = 'cnb'
    cnb_service = ExchangeRateTestConfig.create_service
    
    # For CNB, we need to convert USD -> CZK -> EUR
    # EUR rate is 24.930 CZK per 1 EUR
    # USD rate is 23.117 CZK per 1 USD
    # So 1 USD -> 23.117 CZK -> 0.9272 EUR (23.117/24.930)
    # And 100 USD -> 92.72 EUR
    
    # Mock the specific get_rate call we need for this test
    allow(cnb_service).to receive(:get_rate).with('USD', 'EUR').and_return(
      ExchangeRate.new(
        from: Currency.new('USD'),
        to: Currency.new('EUR'),
        rate: 0.9272,
        date: Date.today
      )
    )
    
    cnb_result = cnb_service.convert(100, 'USD', 'EUR')
    expect(cnb_result[:converted_amount]).to be_within(0.01).of(92.72)
    
    # Test with ECB provider
    ExchangeRateTestConfig.provider_type = 'ecb'
    ecb_service = ExchangeRateTestConfig.create_service
    
    # For ECB, we need EUR -> USD rate, which is 1.0832
    # To get USD -> EUR, we use 1/1.0832 = 0.9232
    # So 100 USD -> 92.32 EUR
    
    # Mock the specific get_rate call we need for this test
    allow(ecb_service).to receive(:get_rate).with('USD', 'EUR').and_return(
      ExchangeRate.new(
        from: Currency.new('USD'),
        to: Currency.new('EUR'),
        rate: 0.9232,
        date: Date.today
      )
    )
    
    ecb_result = ecb_service.convert(100, 'USD', 'EUR')
    expect(ecb_result[:converted_amount]).to be_within(0.01).of(92.32)
    
    # Verify both results are close to each other as consistency check
    expect(cnb_result[:converted_amount]).to be_within(1.0).of(ecb_result[:converted_amount])
  end
end 