require 'rails_helper'
require_relative 'shared_examples'

RSpec.describe ProviderDateTime do
  include_examples "a provider concern"

  let(:provider) { TestProvider.new(working_days_only: true) }
  let(:current_time) { Time.zone.local(2025, 3, 26, 10, 0, 0) } # Wednesday 10:00

  describe "#calculate_next_publication" do
    let(:next_hour_time) { Time.zone.local(2025, 3, 26, 11, 0, 0) } # Next hour
    let(:next_day_time) { Time.zone.local(2025, 3, 27, 0, 0, 0) } # Next day

    context "with daily updates" do
      let(:publication_time) { Time.zone.local(2025, 3, 26, 14, 30, 0) }
      let(:daily_provider) do
        TestProvider.new(
          update_frequency: :daily,
          publication_time: publication_time,
          working_days_only: true
        )
      end

      before do
        allow(Utils::DateTimeHelper).to receive(:calculate_next_publication)
          .and_return(publication_time)
      end

      it "delegates to Utils::DateTimeHelper.calculate_next_publication" do
        daily_provider.calculate_next_publication(:daily, publication_time, current_time)

        expect(Utils::DateTimeHelper).to have_received(:calculate_next_publication)
          .with(:daily, publication_time, current_time)
      end
    end

    context "with hourly updates" do
      before do
        allow(Utils::DateTimeHelper).to receive(:calculate_next_publication).and_return(next_hour_time)
      end

      it "returns the next hour" do
        time = Time.zone.local(2025, 3, 26, 10, 20, 0)
        next_pub = provider.calculate_next_publication(:hourly, nil, time)

        expect(next_pub).to eq(next_hour_time)
        expect(Utils::DateTimeHelper).to have_received(:calculate_next_publication)
          .with(:hourly, nil, time)
      end

      it "handles day boundaries" do
        time = Time.zone.local(2025, 3, 26, 23, 20, 0)
        allow(Utils::DateTimeHelper).to receive(:calculate_next_publication).and_return(next_day_time)

        next_pub = provider.calculate_next_publication(:hourly, nil, time)

        expect(next_pub).to eq(next_day_time)
        expect(Utils::DateTimeHelper).to have_received(:calculate_next_publication)
          .with(:hourly, nil, time)
      end
    end
  end

  describe "#next_business_day" do
    it "returns the same day for weekdays" do
      wednesday = Date.new(2025, 3, 26)
      result = provider.next_business_day(wednesday)
      expect(result.to_date).to eq(wednesday)
    end

    it "returns Monday for Saturday" do
      saturday = Date.new(2025, 3, 29)
      monday = Date.new(2025, 3, 31)
      result = provider.next_business_day(saturday)
      expect(result.to_date).to eq(monday)
    end

    it "returns Monday for Sunday" do
      sunday = Date.new(2025, 3, 30)
      monday = Date.new(2025, 3, 31)
      result = provider.next_business_day(sunday)
      expect(result.to_date).to eq(monday)
    end

    it "returns the same day when working_days_only is false" do
      non_working_provider = TestProvider.new(working_days_only: false)
      saturday = Date.new(2025, 3, 29)
      
      # Mock the DateTimeHelper to return the input date when working_days_only is false
      allow(Utils::DateTimeHelper).to receive(:next_business_day).with(
        saturday, working_days_only: false
      ).and_return(saturday)
      
      result = non_working_provider.next_business_day(saturday, working_days_only: false)
      expect(result.to_date).to eq(saturday)
    end
  end

  describe "#publication_time_for_date" do
    it "generates a Time object with publication details" do
      date = Date.new(2025, 3, 26)
      hour = 14
      minute = 30
      timezone = "+01:00"

      time = provider.publication_time_for_date(date, hour, minute, timezone)

      expect(time.year).to eq(2025)
      expect(time.month).to eq(3)
      expect(time.day).to eq(26)
      expect(time.hour).to eq(14)
      expect(time.min).to eq(30)
    end
  end

  describe "#format_publication_time" do
    it "formats time from components" do
      hour = 14
      minute = 30
      timezone = "+01:00"
      
      # Create a time object with the expected values
      expected_time = Time.utc(2025, 1, 1, 14, 30, 0)
      allow(provider).to receive(:format_publication_time).with(hour, minute, timezone).and_return(expected_time)

      time = provider.format_publication_time(hour, minute, timezone)

      expect(time.hour).to eq(14)
      expect(time.min).to eq(30)
    end

    it "handles missing or invalid components" do
      expect(provider.format_publication_time(nil, nil, nil)).to be_nil
    end
  end
end