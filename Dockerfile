# Multi-stage build - Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /app

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release -o /app/publish --no-build

# Multi-stage build - Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Install additional dependencies if needed
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application from builder
COPY --from=builder /app/publish .

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Set environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "AciPlatform.Api.dll"]
