apim-templates extract --extractorConfig apimconfig.json 

dotnet tool list -g
$HOME/.dotnet/tools/apim-templates extract --extractorConfig apimconfig.json 

az storage file upload-batch --account-key JBXN3F8nteA/J0cqpcMmhhKP5nPKQ5Hj8YypACauHqLU54IVNKeDyseebV1yLVpSdhoEexFLjChr0oSuChLUTA== --account-name insurancepipe --destination 'https://insurancepipe.blob.core.windows.net/tps/test/' --source .

$HOME/.dotnet/tools/apim-templates extract --extractorConfig apimconfig.json --fileFolder './extract2'


azcopy copy "./extract" "https://insurancepipe.blob.core.windows.net/tps/test?sv=2020-02-10&ss=b&srt=co&sp=rlx&se=2022-02-17T18:24:29Z&st=2021-02-17T10:24:29Z&spr=https,http&sig=PbJy4QFRaFJ5WgTCBnR7hgSZg77xFnPCH%2B12sbf39VM%3D" --recursive=true


az storage blob directory upload -c tps --account-name insurancepipe -s "./" -d 'https://insurancepipe.blob.core.windows.net/tps/test/' --recursive


- script: | 
    echo "hardcodedsastoken - $(hardcodedsastoken)"
    
    
    for f in *; do
      if [[ ! -n "policies" && -d ${f} && ! -L ${f} ]]; then
        echo "Deploying API: $f"
        # az deployment group create $deploymentName $rg --template-file ./$f/$apiNamePrefix-master.template.json --parameters ./$f/$apiNamePrefix-parameters.json --parameters ApimServiceName=${{ env.ApimServiceName }} LinkedTemplatesBaseUrl=${{ env.LinkedTemplatesBaseUrl }}/$f
      fi
    done

  workingDirectory: src/api/APIM/scripts/extract/
  displayName: 'deploy files'

  bash testscripts.sh
bash aad.sh

  