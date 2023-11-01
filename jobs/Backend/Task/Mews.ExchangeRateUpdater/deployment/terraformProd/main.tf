provider "azurerm" {
    features {}
    skip_provider_registration = "true"
}

terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">= 2.26"
    }
  }

  required_version = ">= 0.14.9"

  backend "azurerm" {
    resource_group_name  = "exchangerateupdater-rg"
    storage_account_name = "exchangerateupdatertfst"
    container_name       = "tfstate"
    key                  = "exchangerateupdater.tfstate"
  }
}