# Shared examples for provider specs

# Test implementation of Provider for spec examples
class TestProvider < BaseProvider
  attr_accessor :test_metadata

  def initialize(metadata = {})
    # Add required base_url for provider config
    test_config = { 'base_url' => 'https://test-provider.example.com/api' }

    super(test_config)

    @test_metadata = metadata

    # Ensure publication time is set for hourly/minute tests
    return unless %i[hourly minute].include?(metadata[:update_frequency])

    @test_metadata[:publication_time] ||= Time.zone.local(2025, 3, 26, 14, 30, 0)
  end

  def fetch_rates
    []
  end

  def metadata
    @test_metadata
  end
end

RSpec.shared_examples "a provider concern" do
  it "is included in BaseProvider" do
    expect(BaseProvider.included_modules).to include(described_class)
  end
end