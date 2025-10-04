using Elasticsearch.Net;
using Nest;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// --- Configuração do Cliente Elasticsearch (NEST) ---
var settings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Uri"]))
    .ApiKeyAuthentication(new ApiKeyAuthenticationCredentials(builder.Configuration["Elasticsearch:ApiKey"]))
    .DefaultIndex("jogos-index"); // Nome do índice que vamos usar para os jogos

var client = new ElasticClient(settings);
builder.Services.AddSingleton<IElasticClient>(client);
// --- Fim da Configuração ---

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- Endpoint de Busca ---
app.MapGet("/busca", async ([Required] string termo, IElasticClient esClient) =>
{
    // A busca "MultiMatch" procura o termo em vários campos ao mesmo tempo.
    var searchResponse = await esClient.SearchAsync<JogoDocument>(s => s
        .Query(q => q
            .MultiMatch(m => m
                .Query(termo)
                .Fields(f => f
                    .Field(p => p.Name, boost: 3) // Damos um "boost" (peso maior) para o nome
                    .Field(p => p.Company)
                )
                .Fuzziness(Fuzziness.Auto) // Permite pequenas correções de digitação (fuzzy search)
            )
        )
    );

    if (!searchResponse.IsValid)
    {
        // Se houver um erro na consulta, o retornamos para depuração
        return Results.Problem(searchResponse.DebugInformation);
    }

    return Results.Ok(searchResponse.Documents);
})
.WithTags("Busca")
.WithSummary("Realiza uma busca por jogos no catálogo indexado.")
.WithOpenApi();

app.Run();

// --- Classe que representa o documento no Elasticsearch ---
public class JogoDocument
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public double Price { get; set; }
    public string Genre { get; set; } // Usar string aqui simplifica a busca
    public string Rating { get; set; }
}