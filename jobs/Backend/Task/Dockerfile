FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ExchangeRates.Api/ExchangeRates.Api.csproj", "ExchangeRates.Api/"]
COPY ["ExchangeRates.Application/ExchangeRates.Application.csproj", "ExchangeRates.Application/"]
COPY ["ExchangeRates.Domain/ExchangeRates.Domain.csproj", "ExchangeRates.Domain/"]
COPY ["ExchangeRates.Infrastructure/ExchangeRates.Infrastructure.csproj", "ExchangeRates.Infrastructure/"]

RUN dotnet restore "ExchangeRates.Api/ExchangeRates.Api.csproj"

COPY . .
WORKDIR "/src/ExchangeRates.Api"
RUN dotnet publish "ExchangeRates.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "ExchangeRates.Api.dll"]
