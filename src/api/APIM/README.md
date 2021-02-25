# AzureAPIM Extraction

```
az login

sourceRG="mondaydemo"
sourceAPIMInstance="insprefixdevAPIM"
destAPIMInstance="insprefixdevAPIM"

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

echo "run clean up to remove un-needed shared templates and policies"
rm ${apisTemplatesFolder}/*
rm ${apisTemplatesFolder}/policies/*
rm -rf ${apisTemplatesFolder}/policies
```

## Deploy Shared Template


```
buildID="shared9999"
storageContainerName="latest7"
# sourceAPIMInstance="insprefixdevAPIM"
storageSASToken="?sv=2020-02-10&ss=b&srt=co&sp=rlx&se=2022-02-17T18:24:29Z&st=2021-02-17T10:24:29Z&spr=https,http&sig=PbJy4QFRaFJ5WgTCBnR7hgSZg77xFnPCH%2B12sbf39VM%3D"
storagelocation="https://insurancepipe.blob.core.windows.net/$storageContainerName/build$buildID"
destinationRG="mondaydemo"
destAPIMInstance="insprefixprodAPIM"

az config set extension.use_dynamic_install=yes_without_prompt

az storage blob directory upload -c $storageContainerName --account-name insurancepipe -s "${sharedTemplatesFolder}/*" -d "build$buildID" --recursive


echco "Deploy APIM master shared template from: ${sharedTemplatesFolder}/${sourceAPIMInstance}-master.template.json"

az deployment group create --name apim_$buildID --resource-group $destinationRG --template-file ${sharedTemplatesFolder}/${sourceAPIMInstance}-master.template.json --parameters ${sharedTemplatesFolder}/$sourceAPIMInstance-parameters.json --parameters ApimServiceName=$destAPIMInstance --parameters LinkedTemplatesBaseUrl=$storagelocation --parameters LinkedTemplatesUrlQueryString=$storageSASToken PolicyXMLSasToken=$storageSASToken PolicyXMLBaseUrl=${storagelocation}/policies 

```

## Deploy Individual APIs

```
buildID="apishhhh"
storagelocation="https://insurancepipe.blob.core.windows.net/$storageContainerName/build$buildID"

az storage blob directory upload -c $storageContainerName --account-name insurancepipe -s "${apisTemplatesFolder}/*" -d "build${buildID}" --recursive

echo "Loop through and deploy individual API's"

cd $apisTemplatesFolder
for d in */ ; do
    echo "Deplopying ./${d}${sourceAPIMInstance}-master.template.json"
    
     az deployment group create --name test --resource-group $destinationRG --template-file ./${d}${sourceAPIMInstance}-master.template.json --parameters ./${d}$sourceAPIMInstance-parameters.json --parameters ApimServiceName=$apimTarget --parameters LinkedTemplatesBaseUrl=$storagelocation --parameters LinkedTemplatesUrlQueryString=$storageSASToken PolicyXMLSasToken=$storageSASToken PolicyXMLBaseUrl=${storagelocation}/policies 
done


```