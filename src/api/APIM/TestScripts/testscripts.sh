
buildID="987653"
storageContainerName="latest7"
sourceAPIMInstance="insprefixdevAPIM"
storageSASToken="?sv=2020-02-10&ss=b&srt=co&sp=rlx&se=2022-02-17T18:24:29Z&st=2021-02-17T10:24:29Z&spr=https,http&sig=PbJy4QFRaFJ5WgTCBnR7hgSZg77xFnPCH%2B12sbf39VM%3D"
storagelocation="https://insurancepipe.blob.core.windows.net/$storageContainerName/build$buildID"
destinationRG="mondaydemo"
apimTarget="insprefixprodAPIM"

az config set extension.use_dynamic_install=yes_without_prompt

az storage blob directory upload -c $storageContainerName --account-name insurancepipe -s "./extract/*" -d "build$buildID" --recursive


echco "Deploy APIM master shared template from: ./extract/${sourceAPIMInstance}-master.template.json"
az deployment group create --name apim_$buildID --resource-group $destinationRG --template-file ./extract/${sourceAPIMInstance}-master.template.json --parameters ./extract/$sourceAPIMInstance-parameters.json --parameters ApimServiceName=$apimTarget --parameters LinkedTemplatesBaseUrl=$storagelocation --parameters LinkedTemplatesUrlQueryString=$storageSASToken PolicyXMLstorageSASToken=$storageSASToken PolicyXMLBaseUrl=${storagelocation}/policies 



echo "Loop through and deploy individual API's"
cd ../apis
for d in */ ; do
    echo "Deplopying ./${d}${sourceAPIMInstance}-master.template.json"
    az deployment group create --name test --resource-group $destinationRG --template-file ./${d}${sourceAPIMInstance}-master.template.json --parameters ./${d}$sourceAPIMInstance-parameters.json --parameters ApimServiceName=$apimTarget --parameters LinkedTemplatesBaseUrl=$storagelocation --parameters LinkedTemplatesUrlQueryString=$storageSASToken PolicyXMLstorageSASToken=$storageSASToken PolicyXMLBaseUrl=${storagelocation}/policies 
done

