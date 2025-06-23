# frozen_string_literal: true

require 'sinatra/base'

class ViewsController < Sinatra::Base
  set :views, File.expand_path('../views', __dir__)

  get '/' do
    erb :index
  end
end
