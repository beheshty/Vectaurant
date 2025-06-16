namespace Vectaurant.Shared;

public class OpenAIOptions
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public ChatBotConfig ChatBot { get; set; }
    public EmbeddingModelConfig EmbeddingModel { get; set; }
}

public class ChatBotConfig
{
    public string ModelId { get; set; }
}

public class EmbeddingModelConfig
{
    public string ModelId { get; set; }
}

public class QdrandOptions
{
    public string Endpoint { get; set; }
}
