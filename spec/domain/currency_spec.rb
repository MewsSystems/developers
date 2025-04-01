require 'rails_helper'

RSpec.describe Currency do
  it "stores the currency code and name" do
    cur = Currency.new('usd', 'United States Dollar')
    expect(cur.code).to eq 'USD'          # code is upcased
    expect(cur.name).to eq 'United States Dollar'
  end

  it "considers currencies equal if their codes match (case-insensitive)" do
    c1 = Currency.new('eur', 'Euro')
    c2 = Currency.new('EUR', 'Eurozone Euro')
    expect(c1).to eq(c2)
    # And they produce the same hash, suitable for use as hash keys
    expect(c1.hash).to eq(c2.hash)
  end

  it "throws an error if code is not provided or empty" do
    expect { Currency.new(nil) }.to raise_error(ArgumentError)
    expect { Currency.new('   ') }.to raise_error(ArgumentError)
  end

  it "has a string representation equal to its code" do
    cur = Currency.new('JPY', 'Japanese Yen')
    expect(cur.to_s).to eq 'JPY'
  end
end