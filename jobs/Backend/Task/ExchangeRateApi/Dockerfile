# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore any dependencies (via NuGet)
COPY *.csproj ./
RUN dotnet restore

# Copy the project files and build for release with Native AOT
COPY . ./
RUN dotnet publish -c Release -o out -r linux-x64 --self-contained true --sc true -p:PublishAot=true

# Generate runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app runs on
EXPOSE 80

# Start the application
ENTRYPOINT ["./ExchangeRateApi"]
