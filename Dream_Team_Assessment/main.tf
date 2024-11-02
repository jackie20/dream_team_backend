provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "main" {
  name     = "prod-rg-001"               # Resource group name
  location = "West Europe"                # Azure region for deployment
}

resource "azurerm_app_service_plan" "main" {
  name                = "prod-app-service-plan"  # App Service Plan name
  location            = "West Europe"             # Location of the App Service Plan
  resource_group_name = "prod-rg-001"             # Resource Group name

  sku {
    tier = "Basic"                           # Pricing tier
    size = "B1"                              # Size of the App Service Plan
  }

  per_site_scaling       = false
  maximum_number_of_workers = 1
}

resource "azurerm_app_service" "main" {
  name                = "prod-web-app-001"  # Web App name
  location            = "West Europe"        # Location of the Web App
  resource_group_name = "prod-rg-001"        # Resource Group name
  app_service_plan_id = azurerm_app_service_plan.main.id  # Reference to the App Service Plan

  site_config {
    always_on        = true                 # Keep the app always on
    min_tls_version  = "1.2"                # Minimum TLS version for security
  }

  app_settings = {
    "WEBSITE_NODE_DEFAULT_VERSION" = "14"  # Node.js version
    "DATABASE_URL"                  = "Server=tcp:prod-db-server.database.windows.net;Database=prod-db;User ID=admin@prod-db-server;Password=Pa$$w0rd123;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
    "APPLICATIONINSIGHTS_INSTRUMENTATIONKEY" = "f86d6f63-9f5b-42b7-8e71-0c7ed8cf2e15"
  }
}
