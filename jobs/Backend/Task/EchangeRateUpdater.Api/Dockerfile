FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
# Publish the API project by specifying the .csproj file present in the build context
RUN dotnet publish "EchangeRateUpdater.Api/EchangeRateUpdater.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "EchangeRateUpdater.Api.dll"]
