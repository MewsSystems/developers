#!/bin/bash

echo "RESOURCE_NAME : $1" #RESOURCE_NAME
echo "RESOURCE_GROUP : $2" #RESOURCE_GROUP
echo "RESOURCE_KV : $3" #RESOURCE_KV
echo "JUNO_KV : $4" #JUNO_KV
echo "CONFIG_URL : $5" #CONFIG_URL

echo "Getting $2/$1 principalId .."
RSC_ID=$(az webapp identity show -g $2 -n $1  | jq -r '.principalId')
echo ${RSC_ID}

echo "Assigning permissions to $3 .."
az keyvault set-policy -n $3 --object-id $RSC_ID --secret-permissions get list

echo "Assigning permissions to $4 .."
az keyvault set-policy -n $4 --object-id $RSC_ID --secret-permissions get list

echo "Assigning permissions to config .."
MSYS_NO_PATHCONV=1 az role assignment create --assignee-object-id $RSC_ID --assignee-principal-type ServicePrincipal --role "App Configuration Data Reader" --scope $5

echo "Assigning permissions to junocr .."
MSYS_NO_PATHCONV=1 az role assignment create --assignee-object-id $RSC_ID --assignee-principal-type ServicePrincipal --role "AcrPull" --scope /subscriptions/e1afd5ed-d8c4-48c4-a666-cf03df738780/resourceGroups/Juno-rg/providers/Microsoft.ContainerRegistry/registries/junocr
