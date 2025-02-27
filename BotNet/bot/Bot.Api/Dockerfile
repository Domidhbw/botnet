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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.Api.dll"]