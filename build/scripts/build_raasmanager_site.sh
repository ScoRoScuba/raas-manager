#!bin/bash

set -e

echo "OFX.RAASManager :: Build Script Started"
echo "OFX.RAASManager :: restoring..."

echo "OFX.RAASManager :: cleanup previous artifacts ..."
rm -rf artifacts

echo "OFX.RAASManager :: restore and build solution ..."
dotnet restore src/OFX.RAASManager/OFX.RAASManager.csproj --source "https://api.nuget.org/v3/index.json;https://artifactory.ofx.com/artifactory/api/nuget/ozforex-dev-all" --no-cache --verbosity normal

echo "OFX.RAASManager :: cleaning up prev TestResults folder ..."
rm -rf src/OFX.RAASManager.UnitTests/TestResults
rm -rf TestResults

echo "OFX.RAASManager :: running xunit tests..."
dotnet test src/OFX.RAASManager.UnitTests/OFX.RAASManager.UnitTests.csproj "--logger:trx;LogFileName=xunit-results.trx"

echo "OFX.RAASManager :: running xunit tests..."
dotnet test src/OFX.RAASManager.API.IntegrationTests/OFX.RAASManager.API.IntegrationTests.csproj "--logger:trx;LogFileName=xunit-apiresults.trx"


echo "OFX.RAASManager :: Copy xunit test results to root"
cp -r src/OFX.RAASManager.UnitTests/TestResults/ TestResults

cd /sln

echo "OFX.RAASManager :: publishing..."
echo "OFX.RAASManager :: cleaning up prev published folder ..."
rm -rf build/artifacts/publish

# echo "OFX.Static-Rates :: run all tests under `tests` folder ..."
# find -type f -wholename ./tests/*.csproj | xargs -i dotnet test {} --logger trx -r $(pwd)/artifacts/TestResults --verbosity normal

echo "OFX.RAASManager :: publishing linux version ..."
dotnet publish src/OFX.RAASManager/OFX.RAASManager.csproj -c release -o $(pwd)/build/artifacts/publish/linux-x64 -r linux-x64 --verbosity normal

echo "OFX.RAASManager :: publishing windows version ..."
echo "OFX.RAASManager :: packing nuget package ..."
mkdir -p '$(pwd)/build/artifacts/publish/nuget'
dotnet publish src/OFX.RAASManager/OFX.RAASManager.csproj -c release -o $(pwd)/build/artifacts/publish/win10-x64 --verbosity normal

echo "RAASManager.Site :: done"
