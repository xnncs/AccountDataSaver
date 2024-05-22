FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AccountDataSaver.Api/AccountDataSaver.Api.csproj", "AccountDataSaver.Api/"]
RUN dotnet restore "AccountDataSaver.Api/AccountDataSaver.Api.csproj"
COPY . .
WORKDIR "/src/AccountDataSaver.Api"
RUN dotnet build "AccountDataSaver.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AccountDataSaver.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountDataSaver.Api.dll"]
