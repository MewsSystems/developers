require 'rails_helper'

RSpec.describe ExchangeRateRepository do
  let(:repo) { ExchangeRateRepository.new }
  let(:date) { Date.new(2025, 3, 26) }
  let(:rates) { [double('ExchangeRate')] }

  it "raises NotImplementedError for fetch_for" do
    expect { repo.fetch_for(date) }.to raise_error(NotImplementedError)
  end

  it "raises NotImplementedError for save_for" do
    expect { repo.save_for(date, rates) }.to raise_error(NotImplementedError)
  end
end 