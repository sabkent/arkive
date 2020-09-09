FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ./Server/*.csproj .
RUN dotnet restore
COPY . .

FROM build AS publish
WORKDIR /src/Server
RUN dotnet publish -c Release -o /src/publish
COPY ./Server/workmanagement.db ./src/publish
RUN dotnet ef database update --project ./server/pws.server.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet pws.Server.dll