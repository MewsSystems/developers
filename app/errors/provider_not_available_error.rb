# frozen_string_literal: true

class ProviderNotAvailableError < StandardError
  def initialize(source)
    super("API from provider #{source} is not available")
  end
end
