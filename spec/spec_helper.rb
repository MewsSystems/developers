require 'rails_helper'
require 'rack/test'
require 'json'
require 'webmock/rspec'
require 'simplecov'

# Require support files
Dir[File.join(File.dirname(__FILE__), 'support', '**', '*.rb')].each { |f| require f }

# Load application files
require_relative '../app/domain/currency'
require_relative '../app/domain/exchange_rate'
require_relative '../app/errors/exchange_rate_errors'
require_relative '../app/providers/exchange_rate_provider_interface'
require_relative '../app/providers/base_provider'
require_relative '../app/providers/cnb_provider'
require_relative '../app/providers/ecb_provider'
require_relative '../app/adapters/base/base_adapter'
require_relative '../app/adapters/formats/json_adapter'
require_relative '../app/adapters/formats/xml_adapter'
require_relative '../app/adapters/formats/txt_adapter'
require_relative '../app/adapters/providers/cnb/cnb_adapter'
require_relative '../app/adapters/adapter_factory'
require_relative '../app/fetchers/http_fetcher'
require_relative '../app/services/exchange_rate/rate_service'
require_relative '../app/dtos/exchange_rate_dto'

SimpleCov.start 'rails' do
  add_filter '/bin/'
  add_filter '/db/'
  add_filter '/tasks/'
  add_filter '/test/'
  add_filter '/config/'
  add_filter '/vendor/'
  add_filter '/app/channels/'
  add_filter '/app/jobs/'
  add_filter '/app/mailers/'
  add_filter '/lib/tasks/'
end

RSpec.configure do |config|
  config.expect_with :rspec do |expectations|
    expectations.include_chain_clauses_in_custom_matcher_descriptions = true
  end

  config.mock_with :rspec do |mocks|
    mocks.verify_partial_doubles = true
  end

  config.shared_context_metadata_behavior = :apply_to_host_groups
  config.filter_run_when_matching :focus
  config.example_status_persistence_file_path = "spec/examples.txt"
  config.disable_monkey_patching!

  config.order = :random
  Kernel.srand config.seed

  # Include test helpers
  config.include ExchangeRateTestHelper

  # Disable real HTTP connections in tests
  WebMock.disable_net_connect!(allow_localhost: true)

  # Setup
  config.before(:each) do
    # Reset test configuration before each test
    ExchangeRateTestConfig.reset

    # Reset WebMock
    WebMock.reset!

    # Stub common HTTP requests
    stub_request(:get, /www.cnb.cz/).
      to_return(status: 200, body: sample_cnb_text_data, headers: { 'Content-Type' => 'text/plain' })

    stub_request(:get, /api.cnb.cz/).
      to_return(status: 200, body: sample_cnb_json_data, headers: { 'Content-Type' => 'application/json' })

    stub_request(:get, /www.ecb.europa.eu/).
      to_return(status: 200, body: sample_ecb_xml_data, headers: { 'Content-Type' => 'application/xml' })

    stub_request(:get, /example.com\/rates\.xml/).
      to_return(status: 200, body: sample_ecb_xml_data, headers: { 'Content-Type' => 'application/xml' })

    stub_request(:get, /example.com\/rates$/).
      to_return(status: 200, body: sample_cnb_text_data, headers: { 'Content-Type' => 'text/plain' })
  end

  # Expose methods for switching providers during tests
  config.extend Module.new {
    def with_provider(provider)
      context "with #{provider} provider" do
        around do |example|
          ExchangeRateTestConfig.with_provider(provider) do
            example.run
          end
        end

        yield
      end
    end
  }
end