#!/bin/bash
set -e

echo "1. Spin up the docker build image"

docker run -i --rm -v "$(pwd):/sln" --workdir /sln microsoft/dotnet:2.2-sdk sh build/scripts/build_raasmanager_site.sh

echo "Finished DotNetBuild"
