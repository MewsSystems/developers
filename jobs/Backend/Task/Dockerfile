# Use official .NET runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0

WORKDIR /app

COPY ./publish/ .

ENTRYPOINT ["dotnet", "ExchangeRateUpdater.dll"]
