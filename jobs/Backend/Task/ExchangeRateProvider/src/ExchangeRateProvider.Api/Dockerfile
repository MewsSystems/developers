FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files 
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/ExchangeRateProvider.Api/ExchangeRateProvider.Api.csproj", "src/ExchangeRateProvider.Api/"]
COPY ["src/ExchangeRateProvider.Application/ExchangeRateProvider.Application.csproj", "src/ExchangeRateProvider.Application/"]
COPY ["src/ExchangeRateProvider.Domain/ExchangeRateProvider.Domain.csproj", "src/ExchangeRateProvider.Domain/"]
COPY ["src/ExchangeRateProvider.Infrastructure/ExchangeRateProvider.Infrastructure.csproj", "src/ExchangeRateProvider.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/ExchangeRateProvider.Api/ExchangeRateProvider.Api.csproj"

# Copy all source files
COPY ["src/ExchangeRateProvider.Api/", "ExchangeRateProvider.Api/"]
COPY ["src/ExchangeRateProvider.Application/", "ExchangeRateProvider.Application/"]
COPY ["src/ExchangeRateProvider.Domain/", "ExchangeRateProvider.Domain/"]
COPY ["src/ExchangeRateProvider.Infrastructure/", "ExchangeRateProvider.Infrastructure/"]

# Build and publish
WORKDIR "/src/ExchangeRateProvider.Api"
RUN dotnet publish "ExchangeRateProvider.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ExchangeRateProvider.Api.dll"]