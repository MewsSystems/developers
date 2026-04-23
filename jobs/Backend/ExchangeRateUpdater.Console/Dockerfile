# Use the .NET 6 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY MyApp.Console.csproj ./
RUN dotnet restore

# Copy the rest of the application files and publish the application
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use the .NET 6 runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Console.dll"]
