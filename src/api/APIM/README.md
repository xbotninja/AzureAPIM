# AzureAPIM Extraction

```
az login

sourceRG="tuesdayAPIM"
sourceAPIMInstance="tuesapidevAPIM"
destAPIMInstance="tuesapidevAPIM"

```

## Extract Main Template
```
sharedTemplatesFolder="./SharedTemplates"

echo "Extract shared template to: $sharedTemplatesFolder"    
$HOME/.dotnet/tools/apim-templates extract --extractorConfig extractionConfig.json --fileFolder $sharedTemplatesFolder --sourceApimName $sourceAPIMInstance --destinationApimName $destAPIMInstance --resourceGroup $sourceRG

echo "*********** Shared templates extracted ***********"
```

## Extract individual API's 
```
apisTemplatesFolder="./APIs"
echo "Extract shared template to: $apisTemplatesFolder"
$HOME/.dotnet/tools/apim-templates extract --extractorConfig extractionConfig.json --fileFolder $apisTemplatesFolder --sourceApimName $sourceAPIMInstance --destinationApimName $destAPIMInstance --resourceGroup $sourceRG

echo "*********** Individual templates extracted ***********"

echo "run clean up to remove un-needed shared templates and policies. Also Shared APIs"
# rm ${apisTemplatesFolder}/*
# rm ${apisTemplatesFolder}/policies/*
# rm -rf ${apisTemplatesFolder}/policies
# rm ${apisTemplatesFolder}/echo-api/*
# rm -rf ${apisTemplatesFolder}/echo-api
```

## Deploy Shared Template


```
buildID="shared123456"
storageContainerName="thursdaydemo"
# sourceAPIMInstance="insprefixdevAPIM"
storagelocation="https://insurancepipe.blob.core.windows.net/$storageContainerName/build$buildID"
destinationRG="mondaydemo"
destAPIMInstance="insprefixprodAPIM"

# Generate a SAS token for a storage container that expires in 30 mins
end=`date -u -d "30 minutes" '+%Y-%m-%dT%H:%MZ'`
storageSASToken=`az storage container generate-sas --account-name insurancepipe -n $storageContainerName --https-only --permissions dlrw --expiry $end -o tsv`
echo "Sas Token: $storageSASToken"  

az config set extension.use_dynamic_install=yes_without_prompt

az storage blob directory upload -c $storageContainerName --account-name insurancepipe -s "${sharedTemplatesFolder}/*" -d "build$buildID" --recursive


echco "Deploy APIM master shared template from: ${sharedTemplatesFolder}/${sourceAPIMInstance}-master.template.json"

az deployment group create --name apim_$buildID --resource-group $destinationRG --template-file ${sharedTemplatesFolder}/${sourceAPIMInstance}-master.template.json --parameters ${sharedTemplatesFolder}/$sourceAPIMInstance-parameters.json --parameters ApimServiceName=$destAPIMInstance --parameters LinkedTemplatesBaseUrl=$storagelocation --parameters LinkedTemplatesUrlQueryString=$storageSASToken PolicyXMLSasToken=$storageSASToken PolicyXMLBaseUrl=${storagelocation}/policies 

```

## Deploy Individual APIs

```
buildID="apis123456"
storagelocation="https://insurancepipe.blob.core.windows.net/$storageContainerName/build$buildID"

az storage blob directory upload -c $storageContainerName --account-name insurancepipe -s "${apisTemplatesFolder}/*" -d "build${buildID}" --recursive

echo "Loop through and deploy individual API's"

cd $apisTemplatesFolder
for d in */ ; do
    echo "******************** Deplopying ./${d}${sourceAPIMInstance}-master.template.json **********************"
    trimmedDirectory=$(echo $d | sed 's:/*$::')
      az deployment group create --name test --resource-group $destinationRG --template-file ./${d}${sourceAPIMInstance}-master.template.json --parameters ./${d}$sourceAPIMInstance-parameters.json --parameters ApimServiceName=$destAPIMInstance --parameters LinkedTemplatesBaseUrl=$storagelocation/${trimmedDirectory} --parameters LinkedTemplatesUrlQueryString=$storageSASToken PolicyXMLSasToken=$storageSASToken PolicyXMLBaseUrl=${storagelocation}/${trimmedDirectory}/policies 
done

cd ../

```

