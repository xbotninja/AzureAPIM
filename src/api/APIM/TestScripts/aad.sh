echo "Setting subscription"
subscriptionId=3d3a0fec-2648-40a0-8f3a-f9fc5a5f3db0
az account set --subscription  $subscriptionId


serverappname="aaServerApp6"
clientappname="aaClientApp6"

echo "Creating Server Application Called $serverappname"
az ad app create --display-name $serverappname 

serverappId=$(az ad app list --display-name $serverappname --query [].appId -o tsv)
echo "$serverappname ApplicationID: $serverappId"

echo "creat service principle for the App"
az ad sp create --id $serverappId

echo "set application ID URI"
az ad app update --id $serverappId --identifier-uris "api://$serverappId"


echo "Assigning Read Role to Server App"
az ad app update --id $serverappId --app-roles @readrole.json

echo "created server app"


echo "Creating Client Application Called $clientappname"
az ad app create --display-name $clientappname 

clientappId=$(az ad app list --display-name $clientappname --query [].appId -o tsv)
echo "$clientappname ApplicationID: $clientappId"

echo "creat service principle for the App"
az ad sp create --id $clientappId


approleId=$(az ad sp show --id $serverappId --query "appRoles[].id" -o tsv)


az ad app permission add --id $clientappId --api $serverappId --api-permissions $approleId

