.PHONY: up rebuild down logs test-unit test-integration test-all run-local-fetcher run-local-api

help: ## Show this help message
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | \
	  awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-25s\033[0m %s\n", $$1, $$2}'

up: ## Start the Exchange Rate Updater service
	docker compose up -d

rebuild: ## Rebuild and Start the Exchange Rate Updater service
	docker compose up -d --build

down: ## Stop and remove the Exchange Rate Updater service containers
	docker compose down -v

logs: ## Check logs
	docker compose logs -f

test-unit: ## Run Unit Tests
	dotnet test Mews.ExchangeRateUpdater.Domain.UnitTests
	dotnet test Mews.ExchangeRateUpdater.Application.UnitTests
	dotnet test Mews.ExchangeRateUpdater.Infrastructure.UnitTests

test-integration: ## Run Integration Tests
	dotnet test Mews.ExchangeRateUpdater.Infrastructure.IntegrationTests

test-all: test-unit test-integration ## Run all tests

run-local-fetcher: ## Run local fetcher project
	dotnet build
	dotnet run --project Mews.ExchangeRateUpdater.Fetcher

run-local-api: ## Run local API project
	dotnet build
	dotnet run --project Mews.ExchangeRateUpdater.API

