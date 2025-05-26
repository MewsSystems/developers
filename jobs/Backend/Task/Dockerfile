# Use the official .NET runtime image for production
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY Task/*.csproj ./Task/
COPY ExchangeRate.Tests/*.csproj ./ExchangeRate.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src/Task
RUN dotnet publish -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.dll"]