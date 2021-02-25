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

```

## Extract individual API's 
```
apisTemplatesFolder="./APIs"


echo "Extract shared template to: $apisTemplatesFolder"
$HOME/.dotnet/tools/apim-templates extract --extractorConfig extractionConfig.json --fileFolder $apisTemplatesFolder --sourceApimName $sourceAPIMInstance --destinationApimName $destAPIMInstance --resourceGroup $sourceRG

#find $sharedTemplatesFolder -name policies -exec rm -rf {} \;
#find $apisTemplatesFolder -type d -name policies -exec rmdir {} \;

rm -rf ${apisTemplatesFolder}/policies
rm ${apisTemplatesFolder}/*
```
