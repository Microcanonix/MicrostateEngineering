# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MainConsole/MainConsole.csproj", "MainConsole/"]
COPY ["Molecules/ResearchDefinition/ResearchDefinitionRepository/ResearchDefinitionRepository.csproj", "Molecules/ResearchDefinition/ResearchDefinitionRepository/"]
COPY ["Utilities/IUtilitiesServices/IUtilitiesServices.csproj", "Utilities/IUtilitiesServices/"]
COPY ["Molecules/ResearchDefinition/IResearchDefinitionRepository/IResearchDefinitionRepository.csproj", "Molecules/ResearchDefinition/IResearchDefinitionRepository/"]
COPY ["Molecules/ResearchDefinition/ResearchDefinitionDomain/ResearchDefinitionDomain.csproj", "Molecules/ResearchDefinition/ResearchDefinitionDomain/"]
COPY ["Molecules/ResearchDefinition/ResearchDefinitionService/ResearchDefinitionService.csproj", "Molecules/ResearchDefinition/ResearchDefinitionService/"]
COPY ["Molecules/ResearchDefinition/IResearchDefintionService/IResearchDefintionService.csproj", "Molecules/ResearchDefinition/IResearchDefintionService/"]
COPY ["Utilities/UtilitiesServices/UtilitiesServices.csproj", "Utilities/UtilitiesServices/"]
RUN dotnet restore "./MainConsole/MainConsole.csproj"
COPY . .
WORKDIR "/src/MainConsole"
RUN dotnet build "./MainConsole.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MainConsole.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainConsole.dll"]