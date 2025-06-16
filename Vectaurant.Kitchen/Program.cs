using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Vectaurant.Kitchen;
using Vectaurant.Shared;


var openAiConfig = ConfigurationLoader.GetSection<OpenAIOptions>(nameof(OpenAIOptions));
var chatModelId = openAiConfig.ChatBot.ModelId;
var endpoint = openAiConfig.Endpoint;
var apiKey = openAiConfig.ApiKey;
var embeddingModelId = openAiConfig.EmbeddingModel.ModelId;
var qdrandConfig = ConfigurationLoader.GetSection<QdrandOptions>(nameof(QdrandOptions));
var qdrantEndpoint = qdrandConfig.Endpoint;

var builder = Kernel.CreateBuilder();
//builder.Services.AddAzureOpenAIEmbeddingGeneration(embeddingModelId, azureEndpoint, azureApiKey);
var openAIClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
builder.AddAzureOpenAIChatCompletion(chatModelId, openAIClient);
var embeddingClient = openAIClient.GetEmbeddingClient(embeddingModelId).AsIEmbeddingGenerator();
// text-embedding-3-large has 3072 dimensions
builder.Services.AddQdrantVectorStore(qdrantEndpoint, https: false);

var kernel = builder.Build();
var vectorStore = kernel.GetRequiredService<QdrantVectorStore>();

// 3. Define the data to be indexed
List<MenuItem> menuItems = MenuProvider.GetMenuItems();


