# frozen_string_literal: true

require 'sinatra'
require_relative 'config/environment'
require_relative 'app/controllers/api/exchange_rates_controller'
require_relative 'app/controllers/views_controller'

class ExchangeRateApp < Sinatra::Base
  set :public_folder, File.expand_path('public', __dir__)

  use Api::ExchangeRatesController
  use ViewsController
end
