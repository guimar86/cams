FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["cams.api/cams.api.csproj", "cams.api/"]
COPY ["cams.application/cams.application.csproj", "cams.application/"]
COPY ["cams.contracts/cams.contracts.csproj", "cams.contracts/"]
RUN dotnet restore "cams.api/cams.api.csproj"
COPY . .
WORKDIR "/src/cams.api"
RUN dotnet build "./cams.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./cams.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cams.api.dll"]
