data "azurerm_resource_group" "exchangerateupdater-rg" {
  name     = var.rg-name
}

data "azurerm_service_plan" "exchangerateupdater-asp" {
  name                = var.asp-name
  resource_group_name = var.asp-rg-name
}

resource "azurerm_linux_web_app" "exchangerateupdater-api" {
  name                = var.app-name
  location            = var.location
  resource_group_name = var.rg-name
  service_plan_id     = data.azurerm_service_plan.exchangerateupdater-asp.id
  https_only          = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    cors {
      allowed_origins = ["*"]
    }
    always_on         = true
    ftps_state        = "Disabled"
  }

  app_settings        = var.app_settings
}