using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

var settings = new ElasticsearchClientSettings(new Uri(builder.Configuration["Elasticsearch:Uri"]))
    .Authentication(new ApiKey(builder.Configuration["Elasticsearch:ApiKey"]));

var client = new ElasticsearchClient(settings);
builder.Services.AddSingleton(client); // Registra o cliente diretamente

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/busca", async ([Required] string termo, ElasticsearchClient esClient) =>
{
    var searchResponse = await esClient.SearchAsync<JogoDocument>(s => s
        .Index("jogos-index") // Especifica o índice na busca
        .Query(q => q
            .MultiMatch(m => m
                .Query(termo)
                .Fields("name^3,company") // "name^3" dá um boost (peso 3x maior) para o campo nome
                .Fuzziness("AUTO")
            )
        )
    );

    if (!searchResponse.IsSuccess())
    {
        return Results.Problem(searchResponse.DebugInformation);
    }

    // Acessamos os documentos através da propriedade .Documents
    return Results.Ok(searchResponse.Documents);
})
.WithTags("Busca")
.WithSummary("Realiza uma busca por jogos no catálogo indexado.")
.WithOpenApi();

app.MapGet("/health", () => new { status = "healthy", timestamp = DateTime.UtcNow }).ExcludeFromDescription();

app.Run();

public class JogoDocument
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public double Price { get; set; }
    public int Genre { get; set; }
    public int Rating { get; set; }
}