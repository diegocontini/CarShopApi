# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CarShopApi.csproj", "./"]
RUN dotnet restore "CarShopApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "CarShopApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CarShopApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' dotnetuser

# Copy published app
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R dotnetuser:dotnetuser /app
USER dotnetuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8000

# Expose port 8000 for Koyeb health checks
EXPOSE 8000

ENTRYPOINT ["dotnet", "CarShopApi.dll"]
