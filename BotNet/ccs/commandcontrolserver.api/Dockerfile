FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandControlServer.Api.dll"]
