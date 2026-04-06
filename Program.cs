using LocalAIAssistant.Endpoints;
using LocalAIAssistant.Models;
using LocalAIAssistant.Services;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

var ollamaEndpoint = new Uri("http://localhost:11434");

builder.Services.AddChatClient(new OllamaChatClient(ollamaEndpoint, "llama3.2"));
builder.Services.AddEmbeddingGenerator(new OllamaEmbeddingGenerator(ollamaEndpoint, "nomic-embed-text"));

builder.Services.AddSingleton<VectorStore>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<AskService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapDocumentEndpoints();
app.MapAskEndpoints();

app.Run();