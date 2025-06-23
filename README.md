# README

# Mews Coding Challenge

This coding challenge is for people who applied to the Backend Developer position at Mews (Ruby on Rails).

## The Challenge requirements

The task is to implement an ExchangeRateProvider for Czech National Bank. The linked example is written in .NET, but it serves only as a reference. Find the data source on their web - part of the task is to find the source of the exchange rate data and a way how to extract it from there.

It is up to you to decide which Ruby gems to use and whether to implement it as a Rails application. Any code design changes/decisions to the provided skeleton are also completely up to you.

The application should expose a simple REST API (ideally JSON). Adding some UI (e.g. via server-rendered pages or a SPA) is a benefit for full-stack applications.

The solution has to be buildable, runnable and the test program should output the obtained exchange rates.

Goal is to implement a fully functional provider based on real world public data source of the assigned bank.

To submit your solution, just open a new pull request to this repository. Alternatively, you can share your repo with Mews interviewers.

Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.

## Technology used

  - SInatra 4.1.1 / Ruby 3.3.6

## Setup

```bash
bundle install
```

## Endpoints

```bash
GET /api/exchange_rates?source=CZK&currencies=EUR,USD,GBP
```
source and currencies are optional parameters. source by default is CZK and all currencies are returned if no currencies parameters is sent

## Solution implementation

At the very first moment I thought about doing this project in Ruby on Rails because is where I feel more comfortable but then I realized that because of the challenge requirements it was not needed to work with a database or work with ActiveRecord models so maybe a Rails was too much code and libraries that this project would not need. So I decided to use a Sinatra because is more light weight and it fits perfect for the project requirements.

I decided to put all the app logic busines inside an app folder. I tried to make the app in a modularized way and tried to be as much uncoupled as possible so I decided to separate controller from services and views, trying to represent the MVC architecture.

There is a controller ExchangeRateController with one endpoint (GET /api/exchanges_rates) and inside this endpoint I validate optional query params source and currencies. Param source is by default CZK because is the only source we have so far but there could be more in the future. Param currencies is a list of currencies just in case the user only wants to check some of them. I added the 5 more "popular" in the view. But in case the param currencies is empty or just not sent then all currencies are listed.

Then we call the service ExchangeRateService with the params source and currencies. This service does a few things:

- Create the provider that is going to give us the data with a ExchangeRateFactoryProvider class. Calling create method with the provided source (In this case 'CZK') the ExchangeRateFactoryProvider is going to return us an instance of a provider. In this case it will be the CzechExchangeRateProvider. This makes possible that if in the future we want to add a new rates provider like for example the national bank of Germany we would just need to add it the PROVIDERS constant. 

```bash
class ExchangeRateProviderFactory
  PROVIDERS = {
    'CZK' => CzechExchangeRateProvider
  }

  def self.create(source)
    provider_class = PROVIDERS[source]
    raise UnsupportedProviderError, source unless provider_class

    provider_class.new
  end
end
```

- Once we have the provider, we call the method request_exchange_rates. This method is defined not in the CzechExchangeRateProvider class but in its parent class ExchangeRateProvider. I have done it this way, trying to take advantage of heritance, in order that in case we need to add another provider in the future we dont need to touch anything of the implementation because every provider will have a method request_exchange_rates available. In this method we use an HHTPClient we have created (lib/http_client.rb) to make the request to the Czech National Bank API to fetch every exchange rate. Is worth to mention that api_url and source_provider are private methods that belong to the subclass provider. This makes possible that every provider added has its own url without care about implementation. And then call the method parse_response that it is defined in CzechExchangeRateProvider. Here we call a XMLParser class that we have created to parse from xml to json given that the Bank's API return the data in this format.

```bash
provider = ExchangeRateProviderFactory.create(source)
rates = provider.request_exchange_rates
```

```bash
class ExchangeRateProvider
  MAX_RETRIES = 2

  def request_exchange_rates
    response_body = HTTPClient.get(api_url, source_provider)
    parse_response(response_body)
  end
```

- Once we have the rates we check if the params currencies is empty or not. If is empty we return the rates given by the Bank and if it is not empty we select only those ones included in currencies param and return the hash object

```bash
return rates if currencies.empty?

rates_filtered = rates[source].select { |rate| currencies.include?(rate[:currency_code]) }

{ source => rates_filtered }
```

It is worth to mention also that we are protecting and handling errors in the case that the source provided it does not exist in the factory, with a custom UnsupportedProviderError, and in the case that the Czech National Bank API is not responding for some reason, with a custom ProviderNotAvailableError. We have to remark as well that the client has 2 retries more in case the request fails.

Then there is a Frontend server rendered with erb that uses a small javascript code to handle checkboxes buttons. Checkboxes can be selected together except when the All checkbox is selected that it causes the rest of the buttons are disabled. If no button is selected then the submit request it behaves as if the All button was selected and returns every rate.

I have done testing with Rspec and every service, provider, factory, controller class has its unitary test where I use stubs to not impact in other dependencies behaviour and an integration test where I test the whole flow together except the call to the external API, that I believe is not needed to test given that is not under our logic business app and it may cause random and punctual problems.

## Improvements

There are some things that could be implemented to improve the exercise, like for example:

- I think a further improvement of this exercise it could be to implement the possibility of sending a specific amount of money and return the equivalent rate in every currency.
