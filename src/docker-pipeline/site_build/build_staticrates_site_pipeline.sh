#!bin/bash

set -e

cd sln

ls

echo "OFX.StaticRates :: Build Script Started"
echo "OFX.StaticRates :: restoring..."

echo "OFX.Static-Rates :: restore and build solution ..."
dotnet restore src/OFX.StaticRates/OFX.StaticRates.csproj --source "https://api.nuget.org/v3/index.json;https://artifactory.ofx.com/artifactory/api/nuget/ozforex-dev-all" --no-cache --verbosity normal

echo "OFX.StaticRates :: cleaning up prev TestResults folder ..."
rm -rf src/OFX.StaticRates.UnitTests/TestResults
rm -rf TestResults

echo "OFX.StaticRates :: running xunit tests..."
dotnet test src/OFX.StaticRates.UnitTests/OFX.StaticRates.UnitTests.csproj "--logger:trx;LogFileName=xunit-results.trx"

echo "OFX.StaticRates :: Copy xunit test results to root"
cp -r src/OFX.StaticRates.UnitTests/TestResults/ TestResults

echo "OFX.StaticRates :: publishing..."
echo "OFX.StaticRates :: cleaning up prev published folder ..."
rm -rf /tmp/staticrates

cd /sln

echo "OFX.StaticRates :: publishing linux version ..."
dotnet publish src/OFX.StaticRates/OFX.StaticRates.csproj -c release -o /tmp/staticrates/linux-x64 -r linux-x64 --verbosity normal

echo "OFX.StaticRates :: publishing windows version ..."
echo "OFX.StaticRates :: packing nuget package ..."
# mkdir -p '/tmp/staticrates/nuget'
dotnet publish src/OFX.StaticRates/OFX.StaticRates.csproj -c release -o /tmp/staticrates/win10-x64 --verbosity normal

ls /tmp/staticrates/linux-x64

echo "StaticRates.Site :: done"
