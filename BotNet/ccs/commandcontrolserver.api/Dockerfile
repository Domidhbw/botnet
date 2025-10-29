FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Bind to a high port so non-root can listen
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY commandcontrolserver.api/CommandControlServer.Api.csproj commandcontrolserver.api/
RUN dotnet restore "commandcontrolserver.api/CommandControlServer.Api.csproj"
COPY . .
WORKDIR "/src/commandcontrolserver.api"
RUN dotnet build "CommandControlServer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommandControlServer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Install curl for the health check and create a non-root user
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && groupadd --gid 10001 appuser \
    && useradd --uid 10001 --gid 10001 --create-home --shell /bin/bash appuser

# Copy app files owned by the non-root user
COPY --from=publish --chown=appuser:appuser /app/publish .

# Drop privileges
USER appuser

# Healthcheck (now hitting :8080 since we're non-root)
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -fsS http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CommandControlServer.Api.dll"]
