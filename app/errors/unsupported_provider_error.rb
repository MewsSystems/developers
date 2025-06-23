# frozen_string_literal: true

class UnsupportedProviderError < StandardError
  def initialize(source)
    super("No provider available for bank from #{source}")
  end
end
