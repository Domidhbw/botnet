# ---------- Runtime base ----------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
# Bring the base OS packages up-to-date to clear Trivy CVEs
RUN apt-get update \
 && apt-get -y dist-upgrade \
 && rm -rf /var/lib/apt/lists/*
EXPOSE 80
EXPOSE 443

# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install Node.js for build only (do NOT copy to runtime)
RUN apt-get update \
 && apt-get install -y --no-install-recommends curl ca-certificates gnupg \
 && curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
 && apt-get install -y --no-install-recommends nodejs \
 && rm -rf /var/lib/apt/lists/*

COPY ["commandcontrolserver_frontend.csproj", "."]
RUN dotnet restore "./commandcontrolserver_frontend.csproj"

COPY . .
RUN dotnet build "./commandcontrolserver_frontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------- Publish ----------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./commandcontrolserver_frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------- Final runtime ----------
FROM base AS final
WORKDIR /app

# Install curl (for healthcheck) and create non-root user
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && groupadd --gid 10001 appuser \
    && useradd --uid 10001 --gid 10001 --create-home --shell /bin/bash appuser

# Copy app as appuser so permissions are correct
COPY --from=publish --chown=appuser:appuser /app/publish .

# Drop privileges
USER appuser

# Healthcheck (adjust path/port if needed)
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -fsS http://localhost/health || exit 1

ENTRYPOINT ["dotnet", "commandcontrolserver_frontend.dll"]
