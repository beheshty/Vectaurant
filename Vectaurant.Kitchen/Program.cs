using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Vectaurant.Shared;
using Vectaurant.Shared.MenuItems;


var openAiConfig = ConfigurationLoader.GetSection<OpenAIOptions>(nameof(OpenAIOptions));
var chatModelId = openAiConfig.ChatBot.ModelId;
var endpoint = openAiConfig.Endpoint;
var apiKey = openAiConfig.ApiKey;
var embeddingModelId = openAiConfig.EmbeddingModel.ModelId;

var qdrandConfig = ConfigurationLoader.GetSection<QdrandOptions>(nameof(QdrandOptions));
var qdrantEndpoint = qdrandConfig.Endpoint;
var qdrantCollectionName = qdrandConfig.CollectionName;

var builder = Kernel.CreateBuilder();

builder.Services.AddQdrantVectorStore(qdrantEndpoint, https: false);

var openAIClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
builder.AddAzureOpenAIChatCompletion(chatModelId, openAIClient);
var embeddingClient = openAIClient.GetEmbeddingClient(embeddingModelId).AsIEmbeddingGenerator();

builder.Services.AddSingleton<MenuItemRepository>();

var kernel = builder.Build();
var vectorStore = kernel.GetRequiredService<QdrantVectorStore>();
var menuRepo = kernel.GetRequiredService<MenuItemRepository>();
var menuItems = menuRepo.GetMenuItems();

Console.WriteLine("Starting to index Menu into Qdrant...");

var collection = vectorStore.GetCollection<ulong, MenuItem>(qdrantCollectionName);
await collection.EnsureCollectionExistsAsync();

var textsToEmbed = menuItems.Select(item =>
    $"Try our {item.Name}, a {item.Category.ToLower()} that {item.Description.ToLower()}. It's available for just ${item.Price}."
).ToList();

var embeddings = await embeddingClient.GenerateAsync(textsToEmbed);

for (int i = 0; i < menuItems.Count; i++)
{
    menuItems[i].TextEmbedding = embeddings[i].Vector;
    await collection.UpsertAsync(menuItems[i]);
    Console.WriteLine($"  - Indexed '{menuItems[i].Name}'");
}

Console.WriteLine("\n - Data indexing complete!");
Console.WriteLine("Press any key to exit!");
Console.ReadKey();

