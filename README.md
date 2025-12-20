# FIAP Cloud Games - Busca API 🔍
# Microsserviço de Busca

Este microsserviço é o motor de consulta da plataforma **FIAP Cloud Games (FCG)**. Ele foi projetado para fornecer buscas de alta performance e baixa latência no catálogo de jogos, utilizando o **Elasticsearch** como provedor de dados.

Na **Fase 4** da Pós-Graduação, este serviço evoluiu de uma arquitetura Serverless/Container Apps para uma infraestrutura robusta orquestrada por **Kubernetes**, com foco total em escalabilidade e resiliência.

## 🚀 Evoluções Técnicas (Fase 4)

Para atender aos requisitos de alta disponibilidade e performance da Fase 4, foram implementadas as seguintes melhorias:

- **Otimização de Imagem Docker**: Migração para a imagem base `aspnet:8.0-bookworm-slim`, resultando em um artefato mais leve, rápido para escalonamento e com menor superfície de ataque.
- **Segurança de Containers**: Implementação de execução com usuário não-root (`USER $APP_UID`), seguindo as melhores práticas de hardening recomendadas para ambientes Kubernetes.
- **Orquestração via AKS**: Preparação total para deploy no **Azure Kubernetes Service**, incluindo manifestos de infraestrutura.
- **Auto Scaling (HPA)**: Configurado para suportar o *Horizontal Pod Autoscaler*, permitindo que a API escale horizontalmente de forma automática baseada no consumo de CPU/Memória.
- **Observabilidade Avançada**: Instrumentação nativa com **New Relic (APM)** para monitoramento de transações, rastreamento de erros e métricas de infraestrutura do cluster.

## 🛠 Tecnologias Utilizadas

- **Runtime**: .NET 8 (C#)
- **Estilo de API**: Minimal APIs
- **Motor de Busca**: Elasticsearch (Elastic Cloud)
- **Conteinerização**: Docker (Multi-stage build)
- **Monitoramento**: New Relic APM
- **Orquestração**: Kubernetes (AKS)

## ⚓ Kubernetes e Resiliência

O microsserviço está configurado para operar dentro do cluster AKS com as seguintes definições de saúde:

- **Liveness Probe**: Monitora se o processo da API está ativo.    
- **Readiness Probe**: Garante que o tráfego só seja enviado para o Pod após a conexão com o Elasticsearch estar estabelecida.    
- **HPA**: Escalabilidade automática garantindo que o sistema suporte picos de alunos jogando simultaneamente sem degradação de performance.

## 📈 Monitoramento (APM)

*   Latência das consultas ao Elasticsearch.
*   Vazão de requisições (Throughput).
*   Saúde dos Pods e utilização de recursos do cluster.

## 🐳 Execução via Docker

A imagem agora é otimizada e segura. Para rodar localmente (certifique-se de configurar as variáveis de ambiente):

```bash
# Build da imagem
docker build -t fiap-cloud-games-busca-api .

# Execução do container
docker run -p 8080:8080 \
  -e ElasticsearchUri="https://sua-url-elastic" \
  -e ElasticsearchApiKey="sua-api-key" \
  fiap-cloud-games-busca-api

