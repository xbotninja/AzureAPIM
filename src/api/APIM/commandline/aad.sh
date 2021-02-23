echo "Setting subscription"
subscriptionId=3d3a0fec-2648-40a0-8f3a-f9fc5a5f3db0
az account set --subscription  $subscriptionId


serverappname="aaServerApp3"

echo "Creating Application Called $serverappname"
az ad app create --display-name $serverappname 

serverappId=$(az ad app list --display-name $serverappname --query [].appId -o tsv)
echo "$serverappname ApplicationID: $serverappId"

echo "set application ID URI"
az ad app update --id $serverappId --identifier-uris "api://$serverappId"


echo "Assigning Read Role to Server App"
az ad app update --id $serverappId --app-roles @readrole.json

echo "done"
#az ad sp show --id serverappId