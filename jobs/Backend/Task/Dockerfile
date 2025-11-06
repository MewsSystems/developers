# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ExchangeRateUpdater.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet build ExchangeRateUpdater.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish ExchangeRateUpdater.csproj -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 ASP.NET runtime image for running web apps
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Create a non-root user for security
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Copy the published application
COPY --from=build /app/publish .

# Set environment variables
ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV CnbExchangeRate__TimeoutSeconds=30
ENV CnbExchangeRate__RetryCount=3
ENV CnbExchangeRate__EnableCache=true
ENV CnbExchangeRate__CacheDurationMinutes=60

# Health check - ping the /health endpoint
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.dll"]
