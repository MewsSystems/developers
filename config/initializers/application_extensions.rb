# Add a services method to the Rails application
module RailsApplicationServices
  def services
    ExchangeRateApplication.container
  end
end

# Extend the Rails application with our services method
Rails.application.singleton_class.prepend RailsApplicationServices 