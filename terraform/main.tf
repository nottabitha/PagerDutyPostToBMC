# Configure the Microsoft Azure Provider
locals {
  authorised_users = [
    "7bb4c9e8-5652-44fa-91ce-4f7585101380", // Wholesale Technology - SRE & DevOps
  ]
}

provider "azurerm" {
   features {
    key_vault {

      purge_soft_delete_on_destroy = true

    }
  }
}

data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "wmsre-pagerduty-req-rg" {

  name     = "wmsre-pagerduty-req-rg"

  location = "Australia Southeast"
}

# Create storage account

resource "azurerm_storage_account" "wmsrepagerdutyreqsa" {

  name                     = "wmsrepagerdutyreqsa"

  resource_group_name      = azurerm_resource_group.wmsre-pagerduty-req-rg.name

  location                 = azurerm_resource_group.wmsre-pagerduty-req-rg.location
  
  account_tier             = "Standard"

  account_replication_type = "LRS"
}

# Create service plan

resource "azurerm_service_plan" "wmsre-pagerduty-req-asp" {

  name                = "azure-pagerduty-req-asp"

  location            = azurerm_resource_group.wmsre-pagerduty-req-rg.location

  resource_group_name = azurerm_resource_group.wmsre-pagerduty-req-rg.name

  os_type = "Linux"

  sku_name = "Y1"
}

# Create function app

module "pagerduty-req-func" {
  source = "github.com/AGLEnergy/redstone.git//modules/func-app"
  name                       = "pagerduty-req-func"
  include_default_webjob_settings = true
  # ask about this vv
  action_group_id                 = azurerm_application_insights.pagerduty-req-appinsights.id
  application_log_alert_rules = null
  # ask about this vv
  allowed_ips = []
  dynamic_metric_alert_rules = null
  application_insights_id         = azurerm_application_insights.pagerduty-req-appinsights.id
  # ask about this vv
  allowed_audiences = []
  static_metric_alert_rules = null
   # ask about this vv
  allowed_subnets = []
  # storage_account_name       = azurerm_storage_account.wmsrepagerdutyreqsa.name

  # storage_account_access_key = azurerm_storage_account.wmsrepagerdutyreqsa.primary_access_key
  
  resource_group_name        = azurerm_resource_group.wmsre-pagerduty-req-rg.name
  app_service_plan_id         = azurerm_service_plan.wmsre-pagerduty-req-asp.id
  location                   = azurerm_resource_group.wmsre-pagerduty-req-rg.location
  app_settings = {
    bmcQaPassword = "@Microsoft.KeyVault(SecretUri=https://bmc-qa-kv.vault.azure.net/secrets/bmc-qa-password/db8c2c874d094aa48c65588dc46d1cb9)"
    bmcQaUsername = "@Microsoft.KeyVault(SecretUri=https://bmc-qa-kv.vault.azure.net/secrets/bmc-qa-username/a36651e28df84fdfb4b317be4af3c96c)"
  }
}

# Create a key vault

module "bmc-qa-kv" {

  source = "github.com/AGLEnergy/redstone.git//modules/key-vault"

  name                    = "bmc-qa-kv"

  resource_group_name     = azurerm_resource_group.wmsre-pagerduty-req-rg.name

  location                = azurerm_resource_group.wmsre-pagerduty-req-rg.location

  network_default_action  = "Allow"

  ip_rules                =  []
  
  tenant_id               = data.azurerm_client_config.current.tenant_id
}

resource "azurerm_key_vault_access_policy" "bmc-qa-kv-ap" {
  for_each = toset(local.authorised_users)

  key_vault_id        = module.bmc-qa-kv.id
  tenant_id           = data.azurerm_client_config.current.tenant_id
  object_id           = each.key

  key_permissions = [
    "Create",
    "Get",
    "List",
    "Delete",
    "Purge",
  ]

  secret_permissions = [
    "Set",
    "Get",
    "List",
    "Delete",
    "Purge",
  ]

  certificate_permissions = [
    "Create",
    "Delete",
    "DeleteIssuers",
    "Get",
    "GetIssuers",
    "Import",
    "List",
    "ListIssuers",
    "ManageContacts",
    "ManageIssuers",
    "SetIssuers",
    "Update",
    "Purge",
  ]
}

resource "azurerm_key_vault_access_policy" "bmc-qa-kv-func-ap" {
  key_vault_id        = module.bmc-qa-kv.id
  tenant_id           = data.azurerm_client_config.current.tenant_id
  object_id           = "be8b7b45-5162-457b-aff5-ed9f8ca8968c"

  key_permissions = [
    "Get",
    "List",
  ]

  secret_permissions = [
    "Get",
    "List",
  ]

  certificate_permissions = [
    "Get",
    "List",
  ]
}


resource "azurerm_application_insights" "pagerduty-req-appinsights" {
  name                = "pagerduty-req-appinsights"
  location            = azurerm_resource_group.wmsre-pagerduty-req-rg.location
  resource_group_name = azurerm_resource_group.wmsre-pagerduty-req-rg.name
  application_type    = "web"
}