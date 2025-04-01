module Debug
  class RedisController < ApplicationController
    # Debug endpoint for Redis testing (development only)
    def index
      return head :forbidden unless Rails.env.development?

      redis_service = Debug::RedisService.new
      result = redis_service.test_operations
      
      render json: result
    end
  end
end 