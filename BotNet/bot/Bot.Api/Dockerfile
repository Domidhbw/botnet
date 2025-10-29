FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Bot.Api/Bot.Api.csproj Bot.Api/
RUN dotnet restore "Bot.Api/Bot.Api.csproj"
COPY . .
WORKDIR "/src/Bot.Api"
RUN dotnet build "Bot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Install curl for health checks and create a non-root user
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && useradd --create-home --shell /bin/bash --uid 10001 appuser

# Copy app files as the non-root user (ensures permissions are correct)
COPY --from=publish --chown=appuser:appuser /app/publish .

# Drop privileges
USER appuser

# Healthcheck: adjust path/port if your app differs
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -fsS http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Bot.Api.dll"]
