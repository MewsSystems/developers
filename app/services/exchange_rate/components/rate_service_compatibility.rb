# Module for test compatibility methods
module RateServiceCompatibility
  def log_unavailable_currency(code, available_codes)
    @currency_validator.log_unavailable_currency(code, available_codes)
  end
  
  def check_currency_availability(requested_currencies, available_rates)
    @currency_validator.check_currency_availability(requested_currencies, available_rates)
  end
end 