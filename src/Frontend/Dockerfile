# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["src/Frontend/Frontend.csproj", "src/Frontend/"]
COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "src/ServiceDefaults/"]
RUN dotnet restore "src/Frontend/Frontend.csproj"

# Copy the rest of the source code
COPY src/Frontend/ src/Frontend/
COPY src/ServiceDefaults/ src/ServiceDefaults/
WORKDIR "/src/src/Frontend"
RUN dotnet build "Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontend.dll"]
