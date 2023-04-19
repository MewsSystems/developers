# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source

COPY . .

RUN dotnet restore "./ExchangeRateUpdater.WebApi.csproj" --disable-parallel
RUN dotnet publish "./ExchangeRateUpdater.WebApi.csproj" -c release -o /app --no-restore

# Server Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.WebApi.dll"]