FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

COPY ["OFX.RAASManager/OFX.RAASManager.csproj", "OFX.RAASManager/"]
COPY ["OFX.RAASManager.Core/OFX.RAASManager.Core.csproj", "OFX.RAASManager.Core/"]
COPY ["OFX.RAASManager.Application/OFX.RAASManager.Application.csproj", "OFX.RAASManager.Application/"]
RUN dotnet restore "OFX.RAASManager/OFX.RAASManager.csproj"
COPY . .
WORKDIR "/src/OFX.RAASManager"
RUN dotnet build "OFX.RAASManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OFX.RAASManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OFX.RAASManager.dll"]


# To create a docker image, run the following command
# docker build . -t ofxraasmanager --force-rm

# To create a container and run it, execute the following command
# docker run --network=docker_bsl -p 60000:60000 -it ofxraasmanager /bin/bash
