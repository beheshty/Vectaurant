using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Vectaurant.FrontDesk.Plugins;
using Vectaurant.Shared;
using Vectaurant.Shared.MenuItems;

var openAiConfig = ConfigurationLoader.GetSection<OpenAIOptions>(nameof(OpenAIOptions));
var chatModelId = openAiConfig.ChatBot.ModelId;
var endpoint = openAiConfig.Endpoint;
var apiKey = openAiConfig.ApiKey;
var embeddingModelId = openAiConfig.EmbeddingModel.ModelId;

var qdrandConfig = ConfigurationLoader.GetSection<QdrandOptions>(nameof(QdrandOptions));
var qdrantEndpoint = qdrandConfig.Endpoint;

var builder = Kernel.CreateBuilder();

builder.Services.AddQdrantVectorStore(qdrantEndpoint, https: false);

var openAIClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
builder.AddAzureOpenAIChatCompletion(chatModelId, openAIClient);
var embeddingClient = openAIClient.GetEmbeddingClient(embeddingModelId).AsIEmbeddingGenerator();
builder.Services.AddSingleton(embeddingClient);

builder.Services.AddSingleton<FrontDeskPlugin>();

builder.Services.AddSingleton<MenuItemRepository>();

var kernel = builder.Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
kernel.Plugins.AddFromObject(kernel.GetRequiredService<FrontDeskPlugin>());

var executionSettings = new OpenAIPromptExecutionSettings()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

const string SystemPrompt =
    "You are a friendly restaurant assistant. Answer the user's questions about the menu items—such as appetizers, main courses, pizzas, desserts, and beverages—by using your tools. " +
    "Collect the items the user wants and add them to their current order. " +
    "When the user indicates they have finished ordering, ask for confirmation and, once confirmed, complete the order. " +
    "After completing the order, say goodbye to the previous customer and begin a new conversation with the next person. " +
    "If you don't know the answer, say so. Do not make up information.";


var history = new ChatHistory(SystemPrompt);

Console.WriteLine("Welcome to Vectaurant! Ask anything about our menu or place an order.");

Console.CancelKeyPress += (_, e) =>
{
    Console.WriteLine("\nExiting Vectaurant. Come back soon!");
    e.Cancel = true;
};

string? userInput;
do
{
    Console.Write("User > ");
    userInput = Console.ReadLine();
    if (string.IsNullOrEmpty(userInput)) break;

    history.AddUserMessage(userInput);

    Console.Write("Assistant > ");
    var result = "";
    await foreach (var c in chatCompletionService.GetStreamingChatMessageContentsAsync(
        history,
        executionSettings,
        kernel))
    {
        result += c.Content;
        Console.Write(c);
    }

    history.AddAssistantMessage(result);
    Console.WriteLine();

} while (true);