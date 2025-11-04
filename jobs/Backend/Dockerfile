FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY Task/. ./
RUN dotnet publish ExchangeRateUpdater.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.dll"]
