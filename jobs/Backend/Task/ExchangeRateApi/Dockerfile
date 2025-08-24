FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ExchangeRateApi/ExchangeRateApi.csproj", "ExchangeRateApi/"]
COPY ["ExchangeRateProviders/ExchangeRateProviders.csproj", "ExchangeRateProviders/"]
RUN dotnet restore "ExchangeRateApi/ExchangeRateApi.csproj"
COPY . .
WORKDIR "/src/ExchangeRateApi"
RUN dotnet publish "ExchangeRateApi.csproj" -c Release -o /app

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ExchangeRateApi.dll"]