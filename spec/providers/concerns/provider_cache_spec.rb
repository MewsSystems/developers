require 'rails_helper'
require_relative 'shared_examples'

RSpec.describe ProviderCache do
  include_examples "a provider concern"

  let(:provider) { TestProvider.new(update_frequency: :hourly) }
  let(:current_time) { Time.new(2025, 3, 26, 10, 0, 0) }

  describe "#cache_ttl" do
    context "with realtime updates" do
      let(:metadata) { { update_frequency: :realtime } }

      it "returns a short TTL (30 seconds)" do
        ttl_defaults = { realtime: 30 }
        allow(Utils::DateTimeHelper).to receive(:get_default_ttls).and_return(ttl_defaults)
        
        expect(provider.cache_ttl(metadata)).to eq(30)
      end
    end

    context "with minute updates" do
      let(:metadata) { { update_frequency: :minute } }

      it "returns a short TTL (30 seconds)" do
        ttl_defaults = { minute: 30 }
        allow(Utils::DateTimeHelper).to receive(:get_default_ttls).and_return(ttl_defaults)
        
        expect(provider.cache_ttl(metadata)).to eq(30)
      end
    end

    context "with hourly updates" do
      let(:metadata) { { update_frequency: :hourly } }

      it "returns a TTL of 15 minutes" do
        ttl_defaults = { hourly: 15 * 60 }
        allow(Utils::DateTimeHelper).to receive(:get_default_ttls).and_return(ttl_defaults)
        
        expect(provider.cache_ttl(metadata)).to eq(15 * 60)
      end
    end

    context "with daily updates and publication time" do
      let(:publication_time) { Time.new(2025, 3, 26, 14, 30, 0) }
      let(:metadata) { 
        { 
          update_frequency: :daily,
          publication_time: publication_time
        } 
      }

      it "calculates TTL until next publication" do
        ttl_defaults = { daily: 24 * 60 * 60 }
        allow(Utils::DateTimeHelper).to receive(:get_default_ttls).and_return(ttl_defaults)
        allow(Utils::DateTimeHelper).to receive(:calculate_ttl_until_next_publication).and_return(4 * 60 * 60) # 4 hours
        
        expect(provider.cache_ttl(metadata, current_time)).to eq(4 * 60 * 60)
        expect(Utils::DateTimeHelper).to have_received(:calculate_ttl_until_next_publication).with(
          :daily, publication_time, current_time, 24 * 60 * 60
        )
      end
    end
  end

  describe "#cache_fresh?" do
    let(:ttl) { 15 * 60 } # 15 minutes in seconds

    it "returns false for nil cached_at" do
      # Bypass BaseProvider's method and call the concern's method directly
      module TestCacheFresh
        include ProviderCache
      end
      test_object = Object.new.extend(TestCacheFresh)
      
      expect(test_object.cache_fresh?(nil, ttl, current_time)).to be false
    end

    context "when cache is fresh" do
      it "returns true" do
        # Bypass BaseProvider's method and call the concern's method directly
        module TestCacheFresh
          include ProviderCache
        end
        test_object = Object.new.extend(TestCacheFresh)
        
        # 5 minutes ago
        cached_at = current_time - 5 * 60
        expect(test_object.cache_fresh?(cached_at, ttl, current_time)).to be true
      end
    end

    context "when cache is stale" do
      it "returns false" do
        # Bypass BaseProvider's method and call the concern's method directly
        module TestCacheFresh
          include ProviderCache
        end
        test_object = Object.new.extend(TestCacheFresh)
        
        # 20 minutes ago
        cached_at = current_time - 20 * 60
        expect(test_object.cache_fresh?(cached_at, ttl, current_time)).to be false
      end
    end
  end

  describe "#execute_with_safe_handling" do
    it "returns the block result when successful" do
      result = provider.execute_with_safe_handling { 42 }
      expect(result).to eq(42)
    end

    it "returns the default value when block raises an error" do
      result = provider.execute_with_safe_handling([]) { raise "Error" }
      expect(result).to eq([])
    end

    it "logs errors when they occur" do
      allow(provider).to receive(:log_message)
      provider.execute_with_safe_handling { raise "Test error" }
      expect(provider).to have_received(:log_message).with(/Test error/, :warn, "TestProvider")
    end
  end
end 