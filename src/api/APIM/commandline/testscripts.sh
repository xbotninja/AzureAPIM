echo "helllo"
for f in **; do
    #if [[ ! -n "policies" && -d ${f} && ! -L ${f} ]]; then
    echo "Deploying API: $f"
    # az deployment group create $deploymentName $rg --template-file ./$f/$apiNamePrefix-master.template.json --parameters ./$f/$apiNamePrefix-parameters.json --parameters ApimServiceName=${{ env.ApimServiceName }} LinkedTemplatesBaseUrl=${{ env.LinkedTemplatesBaseUrl }}/$f
    #fi
done

apimName="insprefixdevAPIM"
sastoken="?sv=2020-02-10&ss=b&srt=co&sp=rlx&se=2022-02-17T18:24:29Z&st=2021-02-17T10:24:29Z&spr=https,http&sig=PbJy4QFRaFJ5WgTCBnR7hgSZg77xFnPCH%2B12sbf39VM%3D"
storageloc="https://insurancepipe.blob.core.windows.net/tps"
rg="mondaydemo"

echo "dirrs------------------------------"
echo " "
for d in */ ; do
    echo "$d"
    echo "./${d}${apimName}-master.template.json"
    az deployment group create --name test --resource-group $rg --template-file ./${d}${apimName}-master.template.json --parameters ./${d}$apimName-parameters.json --parameters LinkedTemplatesBaseUrl=$storageloc --parameters LinkedTemplatesSasToken=$sastoken PolicyXMLSasToken=$sastoken PolicyXMLBaseUrl=${storageloc}/policies 
done

ss