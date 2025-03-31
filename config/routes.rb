Rails.application.routes.draw do
  # API routes - versioned and namespaced
  namespace :api do
    namespace :v1 do
      get '/exchange_rates', to: 'exchange_rates#index'
      get '/exchange_rates/convert', to: 'exchange_rates#convert'
      get '/exchange_rates/currencies', to: 'exchange_rates#currencies'
      get '/exchange_rates/:from/:to', to: 'exchange_rates#show'
    end
  end

  # Legacy routes - map to API endpoints for backward compatibility
  get '/exchange_rates', to: 'api/v1/exchange_rates#index'
  get '/exchange_rates/convert', to: 'api/v1/exchange_rates#convert'
  get '/exchange_rates/supported_currencies', to: 'api/v1/exchange_rates#currencies'

  # Health check for Docker and monitoring
  get '/health', to: 'health#index'
  
  # Debug routes for development only
  if Rails.env.development?
    get '/debug/redis', to: 'health#redis_debug'
  end
end 