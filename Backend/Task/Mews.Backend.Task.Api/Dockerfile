FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Mews.Backend.Task.Api/Mews.Backend.Task.Api.csproj", "Mews.Backend.Task.Api/"]
COPY ["Mews.Backend.Task.Core/Mews.Backend.Task.Core.csproj", "Mews.Backend.Task.Core/"]
RUN dotnet restore "Mews.Backend.Task.Api/Mews.Backend.Task.Api.csproj"
COPY . .
WORKDIR "/src/Mews.Backend.Task.Api"
RUN dotnet build "Mews.Backend.Task.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mews.Backend.Task.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mews.Backend.Task.Api.dll"]