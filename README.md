# Microsserviço de Busca - FIAP Cloud Games 🚀

Este microsserviço faz parte da plataforma FIAP Cloud Games, sendo responsável exclusivamente pela consulta de alta performance no catálogo de jogos indexados no Elasticsearch.

Na Fase 4, este serviço foi evoluído para suportar orquestração via Kubernetes (AKS), com foco em escalabilidade horizontal e monitoramento avançado.

🚀 Evoluções da Fase 4
Docker Otimizado: Migração para a imagem base aspnet:8.0-bookworm-slim, reduzindo o tamanho da imagem e a superfície de ataque.

Segurança: Configuração de usuário não-root (USER $APP_UID) dentro do container para seguir as melhores práticas de segurança de contêineres.

Orquestração: Preparado para execução em cluster Kubernetes (AKS) com suporte a HPA (Horizontal Pod Autoscaler).

Observabilidade: Implementação de APM através do New Relic para monitoramento de performance e saúde do serviço em tempo real.

🛠 Tecnologias Utilizadas
.NET 8 (C#)

Minimal APIs

Elasticsearch (Elastic Cloud)

Docker (Multi-stage builds)

New Relic (APM)

Kubernetes (Orquestração)

🐳 Docker (Padrão Fase 4)
A imagem foi construída utilizando multi-stage build para garantir que o artefato final contenha apenas o necessário para a execução.

Bash

# Para buildar localmente:
docker build -t fiap-cloud-games-busca-api .

# Para rodar (necessário configurar variáveis do Elastic):
docker run -p 8080:8080 \
  -e ElasticsearchUri="SEU_URI" \
  -e ElasticsearchApiKey="SUA_CHAVE" \
  fiap-cloud-games-busca-api
📈 Observabilidade e Performance
Este microsserviço está instrumentado com o New Relic Dotnet Agent. Ele coleta automaticamente:

Tempo de resposta das requisições.

Latência nas consultas ao Elasticsearch.

Consumo de CPU e Memória dentro do cluster Kubernetes.

Rastreamento de erros (Exception Tracking).

⚓ Kubernetes e Escalabilidade
Este serviço possui manifestos para deploy no Azure Kubernetes Service (AKS), configurado com:

Liveness e Readiness Probes: Para garantir que o cluster saiba quando o serviço está saudável.

HPA: Configurado para escalar automaticamente o número de réplicas com base no consumo de CPU, garantindo disponibilidade durante picos de acesso.