FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj", "ExchangeRateUpdater.Api/"]
COPY ["ExchangeRateUpdater.Application/ExchangeRateUpdater.Application.csproj", "ExchangeRateUpdater.Application/"]
COPY ["ExchangeRateUpdater.Domain/ExchangeRateUpdater.Domain.csproj", "ExchangeRateUpdater.Domain/"]
RUN dotnet restore "ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj"
COPY . .
WORKDIR "/src/ExchangeRateUpdater.Api"
RUN dotnet build "ExchangeRateUpdater.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeRateUpdater.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Api.dll"]
