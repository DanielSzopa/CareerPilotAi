# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CareerPilotAi/CareerPilotAi.csproj", "CareerPilotAi/"]
RUN dotnet restore "CareerPilotAi/CareerPilotAi.csproj"

# Copy everything else and build
COPY CareerPilotAi/ CareerPilotAi/
WORKDIR /src/CareerPilotAi
RUN dotnet build "CareerPilotAi.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "CareerPilotAi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Run migrations on startup and then start the application
ENTRYPOINT ["dotnet", "CareerPilotAi.dll"]