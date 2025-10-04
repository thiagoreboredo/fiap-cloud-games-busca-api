# Microsserviço de Busca - FIAP Cloud Games 🚀

Este é o **Microsserviço de Busca** do projeto FIAP Cloud Games. Sua única responsabilidade é fornecer uma API de busca rápida e eficiente para o catálogo de jogos, utilizando o poder do Elasticsearch.

Este serviço não possui banco de dados próprio; ele consome dados indexados no Elasticsearch, que são enviados por outros serviços (como o `catalogo-api`) de forma assíncrona.

---

### 🎯 Responsabilidades do Serviço

-   **Endpoint de Busca**: Expõe um endpoint (`/busca`) que permite aos clientes pesquisar jogos por termos de texto.
-   **Consulta Avançada**: Utiliza consultas `MultiMatch` e `Fuzzy Search` do Elasticsearch para fornecer resultados relevantes, mesmo com pequenos erros de digitação.
-   **Performance**: Projetado para ser leve e rápido, delegando a complexidade da busca para o Elasticsearch.

---

### 🛠️ Tecnologias Utilizadas

-   **.NET 8 (Minimal API)**
-   **Elasticsearch**: Como motor de busca principal.
-   **NEST**: Biblioteca .NET para integração com o Elasticsearch.
-   **Docker**: Para conteinerização da aplicação.