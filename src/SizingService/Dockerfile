# Use the official .NET Aspire workload image as base for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["src/SizingService/SizingService.csproj", "src/SizingService/"]
COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "src/ServiceDefaults/"]
RUN dotnet restore "src/SizingService/SizingService.csproj"

# Copy the rest of the source code
COPY src/SizingService/ src/SizingService/
COPY src/ServiceDefaults/ src/ServiceDefaults/
WORKDIR "/src/src/SizingService"
RUN dotnet build "SizingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SizingService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SizingService.dll"]
