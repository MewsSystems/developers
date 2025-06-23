require 'bundler/setup'
require 'sinatra/base'

Bundler.require(Sinatra::Base.environment)

Dir[File.join(File.dirname(__FILE__), "../app/**/*.rb")].each { |file| require file }

Dir[File.join(File.dirname(__FILE__), "../lib/**/*.rb")].each { |file| require file }
