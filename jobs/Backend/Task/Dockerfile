FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /exchangerate.api
COPY . .
RUN dotnet restore "./src/ExchangeRates.Api/ExchangeRates.Api.csproj"

RUN dotnet build "./src/ExchangeRates.Api/ExchangeRates.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN dotnet test "./tests/ExchangeRates.Api.Tests/ExchangeRates.Api.Tests.csproj"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./src/ExchangeRates.Api/ExchangeRates.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ExchangeRates.Api.dll"]