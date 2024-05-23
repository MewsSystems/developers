FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Run unit tests
RUN dotnet test
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0-bookworm-slim
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Application.dll"]