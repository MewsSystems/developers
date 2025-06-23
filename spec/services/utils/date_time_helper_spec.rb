require 'rails_helper'
require_relative '../../../app/services/utils/date_time_helper'

RSpec.describe Utils::DateTimeHelper do
  describe '.calculate_next_publication' do
    let(:current_time) { Time.new(2023, 11, 15, 10, 0, 0) } # 10:00 AM
    let(:publication_time) { Time.new(2023, 11, 15, 14, 30, 0) } # 2:30 PM

    it 'returns publication time for next hour when frequency is hourly' do
      next_pub = described_class.calculate_next_publication(:hourly, publication_time, current_time)
      expect(next_pub.hour).to eq(11)
      expect(next_pub.min).to eq(30)
    end

    it 'returns publication time for next minute when frequency is minute' do
      next_pub = described_class.calculate_next_publication(:minute, publication_time, current_time)
      expect(next_pub).to eq(current_time + 60)
    end

    it 'returns publication time for today when not passed yet' do
      next_pub = described_class.calculate_next_publication(:daily, publication_time, current_time)
      expect(next_pub.day).to eq(current_time.day)
      expect(next_pub.hour).to eq(14)
      expect(next_pub.min).to eq(30)
    end

    it 'returns publication time for next day when already passed' do
      current_time = Time.new(2023, 11, 15, 15, 0, 0) # 3:00 PM (after publication)
      next_pub = described_class.calculate_next_publication(:daily, publication_time, current_time)
      expect(next_pub.day).to eq(current_time.day + 1)
      expect(next_pub.hour).to eq(14)
      expect(next_pub.min).to eq(30)
    end
  end

  describe '.next_business_day' do
    let(:monday) { Date.new(2023, 11, 13) }    # Monday
    let(:friday) { Date.new(2023, 11, 17) }    # Friday
    let(:saturday) { Date.new(2023, 11, 18) }  # Saturday
    let(:sunday) { Date.new(2023, 11, 19) }    # Sunday

    it 'returns the next day for weekdays' do
      expect(described_class.next_business_day(monday)).to eq(monday)
      expect(described_class.next_business_day(friday)).to eq(friday)
    end

    it 'returns Monday for Saturday' do
      expect(described_class.next_business_day(saturday)).to eq(Date.new(2023, 11, 20)) # Monday
    end

    it 'returns Monday for Sunday' do
      expect(described_class.next_business_day(sunday)).to eq(Date.new(2023, 11, 20)) # Monday
    end

    it 'works with Time objects' do
      saturday_time = Time.new(2023, 11, 18, 10, 0, 0)
      result = described_class.next_business_day(saturday_time)
      expect(result.to_date).to eq(Date.new(2023, 11, 20)) # Monday
      expect(result.hour).to eq(10)
      expect(result.min).to eq(0)
    end
  end

  describe '.determine_fetch_date' do
    let(:today) { Date.today }

    it 'returns today for non-daily updates' do
      metadata = { update_frequency: :hourly }
      expect(described_class.determine_fetch_date(metadata)).to eq(today)
    end

    it 'returns today for daily updates if publication time has passed' do
      now = Time.now
      metadata = {
        update_frequency: :daily,
        publication_time: Time.new(now.year, now.month, now.day, now.hour - 1, 0, 0),
        working_days_only: true
      }

      expect(described_class.determine_fetch_date(metadata)).to eq(today)
    end

    it 'returns previous business day for daily updates if publication time has not passed' do
      now = Time.now
      metadata = {
        update_frequency: :daily,
        publication_time: Time.new(now.year, now.month, now.day, now.hour + 1, 0, 0),
        working_days_only: true
      }

      previous_date = described_class.previous_business_day(today, true)
      expect(described_class.determine_fetch_date(metadata)).to eq(previous_date)
    end
  end
end