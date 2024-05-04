#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Build.props ./
COPY ["ExchangeRateUpdater.API/ExchangeRateUpdater.API.csproj", "ExchangeRateUpdater.API/"]
COPY ["ExchangeRateUpdater.Domain.Core/ExchangeRateUpdater.Domain.Core.csproj", "ExchangeRateUpdater.Domain.Core/"]
COPY ["ExchangeRateUpdater.Domain.Logic/ExchangeRateUpdater.Domain.Logic.csproj", "ExchangeRateUpdater.Domain.Logic/"]
COPY ["ExchangeRateUpdater.Host/ExchangeRateUpdater.Host.csproj", "ExchangeRateUpdater.Host/"]
RUN dotnet restore "./ExchangeRateUpdater.Host/ExchangeRateUpdater.Host.csproj"

COPY . .
WORKDIR "/src/ExchangeRateUpdater.Host"
RUN dotnet build "./ExchangeRateUpdater.Host.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ExchangeRateUpdater.Host.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Host.dll"]
