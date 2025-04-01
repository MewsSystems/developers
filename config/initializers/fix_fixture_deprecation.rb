require 'active_support/testing/file_fixtures'

# Fix for fixture_path= deprecation warnings by redirecting to fixture_paths= when possible
if defined?(ActiveSupport::TestCase) && ActiveSupport::TestCase.respond_to?(:fixture_paths)
  module ActiveSupport
    module TestFixtures
      module ClassMethods
        # Override the deprecated method to use the new one
        def fixture_path=(path)
          self.fixture_paths = [path]
        end
      end
    end
  end
end