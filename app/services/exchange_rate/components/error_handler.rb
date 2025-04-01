require_relative '../../../errors/exchange_rate_errors'

# Class responsible for handling exchange rate errors
class ExchangeRateErrorHandler
  # Handle errors in rate service operations
  # @param error [Exception] The exception to handle
  # @param provider_name [String] The name of the provider
  # @raise [ExchangeRateErrors::Error] A wrapped exchange rate error
  def self.handle_error(error, provider_name)
    raise error if error.is_a?(ExchangeRateErrors::Error)

    # Re-raise ExchangeRateErrors

    # Wrap unexpected errors
    raise ExchangeRateErrors::ServiceError.new(
      "Unexpected error in exchange rate service: #{error.message}",
      error, provider_name
    )
  end
end