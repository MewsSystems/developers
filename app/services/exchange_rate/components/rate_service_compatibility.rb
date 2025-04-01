# Module for test compatibility methods
module RateServiceCompatibility
  delegate :log_unavailable_currency, to: :@currency_validator

  delegate :check_currency_availability, to: :@currency_validator
end