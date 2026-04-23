FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExchangeRateUpdater.ConsoleApp/ExchangeRateUpdater.ConsoleApp.csproj", "ExchangeRateUpdater.ConsoleApp/"]
COPY ["ExchangeRateUpdater.Application/ExchangeRateUpdater.Application.csproj", "ExchangeRateUpdater.Application/"]
COPY ["ExchangeRateUpdater.Domain/ExchangeRateUpdater.Domain.csproj", "ExchangeRateUpdater.Domain/"]
COPY ["ExchangeRateUpdater.Infrastructure/ExchangeRateUpdater.Infrastructure.csproj", "ExchangeRateUpdater.Infrastructure/"]
RUN dotnet restore "ExchangeRateUpdater.ConsoleApp/ExchangeRateUpdater.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/ExchangeRateUpdater.ConsoleApp"
RUN dotnet build "ExchangeRateUpdater.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeRateUpdater.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.ConsoleApp.dll"]
