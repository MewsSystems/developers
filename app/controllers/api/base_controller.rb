# frozen_string_literal: true

require 'sinatra/base'
require 'json'

module Api
  class BaseController < Sinatra::Base
    PREFIX = '/api'

    before do
      content_type :json
    end

    def self.api_route(method, path, &block)
      send(method, "#{PREFIX}#{path}", &block)
    end
  end
end
