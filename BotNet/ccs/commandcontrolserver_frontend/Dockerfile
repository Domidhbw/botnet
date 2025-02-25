FROM node:18 AS node-builder
WORKDIR /app
COPY ClientApp/package*.json ./
RUN npm install
COPY ClientApp ./
RUN npm run build --prefix ClientApp


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dotnet-builder
WORKDIR /src
COPY commandcontrolserver_frontend.csproj ./commandcontrolserver_frontend/
RUN dotnet restore "commandcontrolserver_frontend/commandcontrolserver_frontend.csproj"
COPY . .
WORKDIR "/src/commandcontrolserver_frontend"
RUN dotnet build -c Release
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=dotnet-builder /src/commandcontrolserver_frontend/bin/Release/net7.0/publish/ .

COPY --from=node-builder /app/ClientApp/dist/ClientApp/ /app/wwwroot/

EXPOSE 4000

ENTRYPOINT ["dotnet", "commandcontrolserver_frontend.dll"]
