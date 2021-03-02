

RUN dotnet pack -c Release \
&& dotnet new tool-manifest \
 && dotnet tool install -g --add-source '.\bin\Release' apimtemplate 
