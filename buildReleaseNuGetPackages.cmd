echo "Start building Cynosura ..."
echo "Restore & Build API"
dotnet restore

dotnet pack Cynosura.Core --configuration Release --force --output "..\artifacts"

dotnet pack Cynosura.EF --configuration Release --force --output "..\artifacts"

dotnet pack Cynosura.Web --configuration Release --force --output "..\artifacts"

dotnet pack Cynosura.Messaging --configuration Release --force --output "..\artifacts"

dotnet pack Cynosura.Messaging.Abstractions --configuration Release --force --output "..\artifacts"