FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Busca.API/Busca.API.csproj", "Busca.API/"]
COPY Busca.API.sln .
RUN dotnet restore "Busca.API.sln"
COPY . .
WORKDIR "/src/Busca.API"
RUN dotnet build "Busca.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Busca.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Busca.API.dll"]