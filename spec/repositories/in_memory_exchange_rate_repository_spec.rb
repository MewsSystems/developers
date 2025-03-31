require 'rails_helper'

RSpec.describe InMemoryExchangeRateRepository do
  let(:repo) { InMemoryExchangeRateRepository.new }
  let(:today) { Date.new(2025, 3, 26) }
  let(:tomorrow) { Date.new(2025, 3, 27) }
  let(:czk) { Currency.new('CZK') }
  let(:usd) { Currency.new('USD') }
  let(:rates) { [ExchangeRate.new(from: czk, to: usd, rate: 23.117)] }

  it "returns nil for a date with no saved rates" do
    expect(repo.fetch_for(today)).to be_nil
  end

  it "saves and retrieves rates for a specific date" do
    repo.save_for(today, rates)
    retrieved_rates = repo.fetch_for(today)
    expect(retrieved_rates).to eq(rates)
  end

  it "keeps rates separate for different dates" do
    tomorrow_rates = [ExchangeRate.new(from: czk, to: usd, rate: 23.5)]
    
    repo.save_for(today, rates)
    repo.save_for(tomorrow, tomorrow_rates)
    
    expect(repo.fetch_for(today)).to eq(rates)
    expect(repo.fetch_for(tomorrow)).to eq(tomorrow_rates)
  end

  it "overwrites rates when saving to the same date" do
    new_rates = [ExchangeRate.new(from: czk, to: usd, rate: 24.0)]
    
    repo.save_for(today, rates)
    repo.save_for(today, new_rates)
    
    expect(repo.fetch_for(today)).to eq(new_rates)
  end
end 