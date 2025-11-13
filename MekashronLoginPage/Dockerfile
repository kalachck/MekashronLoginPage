FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MekashronLoginPage.Presentation/MekashronLoginPage.Presentation.csproj", "MekashronLoginPage.Presentation/"]
COPY ["MekashronLoginPage.Application/MekashronLoginPage.Application.csproj", "MekashronLoginPage.Application/"]
RUN dotnet restore "MekashronLoginPage.Presentation/MekashronLoginPage.Presentation.csproj"
COPY . .
WORKDIR "/src/MekashronLoginPage.Presentation"
RUN dotnet build "./MekashronLoginPage.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MekashronLoginPage.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MekashronLoginPage.Presentation.dll"]
