# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de solução (.sln) e o arquivo de projeto (.csproj)
COPY ["Busca.sln", "."]
COPY ["Busca.API/Busca.API.csproj", "Busca.API/"]

# Restaura as dependências
RUN dotnet restore "Busca.sln"

# Copia todo o resto do código fonte
COPY . .
WORKDIR "/src/Busca.API"
RUN dotnet publish "Busca.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 2: Runtime (Otimizado - bookworm-slim para suportar New Relic)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS final
WORKDIR /app
EXPOSE 8080

# Instalação do Agente New Relic (Requisito de Observabilidade)
RUN apt-get update && apt-get install -y --no-install-recommends wget ca-certificates gnupg \
    && echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
    && wget -qO - https://download.newrelic.com/548C16BF.gpg | apt-key add - \
    && apt-get update \
    && apt-get install -y 'newrelic-dotnet-agent' \
    && rm -rf /var/lib/apt/lists/*

# Variáveis de Ambiente para o New Relic
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so

# Segurança: Executar como usuário não-root (Boas práticas K8s)
USER $APP_UID

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Busca.API.dll"]