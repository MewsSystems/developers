variable "location" {
  type        = string
  description = "Azure Region Location"
  default     = "westeurope"
}

variable "asp-rg-name" {
  default = "juno-rg"
}

variable "asp-name" {
  default = "juno-asp-linux"
}

variable "rg-name" {
  default = "exchangerateupdater-rg"
}

variable "app-name" {
  default = "exchangerateupdater-api"
}

variable "app_settings" { 
  type = map(string)

  default = {
    "ASPNETCORE_ENVIRONMENT"                = "Production"
    "DOCKER_REGISTRY_SERVER_URL"            = "https://junocr.azurecr.io/"
    "DOCKER_REGISTRY_SERVER_USERNAME"       = "junocr"
    "DOCKER_REGISTRY_SERVER_PASSWORD"       = "@Microsoft.KeyVault(SecretUri=https://juno-kv.vault.azure.net/secrets/DOCKER-REGISTRY-SERVER-PASSWORD/d32ee6a24aa241f6af64e5f9a78a64f6)"
  }
}

variable "func_settings" { 
  type = map(string)

  default = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"        = "c6863602-95a4-474e-9254-7e121109e0fd"
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = "InstrumentationKey=c6863602-95a4-474e-9254-7e121109e0fd;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/"
    "AZURE_FUNCTIONS_ENVIRONMENT"           = "Production"
    "FUNCTIONS_EXTENSION_VERSION"           = "~4"
  }
}
