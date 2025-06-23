# 🍽️ Vectaurant — Your Smart Restaurant Assistant

**Vectaurant** is an AI-powered restaurant assistant designed to help customers interact with menus and restaurant services more naturally and efficiently. It leverages **OpenAI** for natural language understanding, **Semantic Kernel** for orchestrating plugins and agent behavior, **Qdrant** for vector-based search, and follows the **RAG (Retrieval-Augmented Generation)** pattern to enhance responses with relevant, retrieved information.

---

## 💡 What is Semantic Kernel?
[Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel) is Microsoft’s open-source SDK that lets you easily integrate large language models (LLMs) like OpenAI, Azure OpenAI, or Hugging Face into your .NET apps. It helps you build agents that can reason and interact in natural language.

---

## 🚀 Features

- 🔍 **Natural Language Interaction**  
  Ask questions like *“What’s spicy on the menu?”* — and get smart, helpful answers.

- 🧠 **AI-Powered Agent with Plugins**  
  Uses OpenAI models and Semantic Kernel to understand intent and execute tasks through custom plugins.

- 📦 **Vector Search with Qdrant**  
  Menu items and restaurant knowledge are embedded using OpenAI embedding models and stored in Qdrant for fast semantic search.

---

## 🧱 Tech Stack

- **.NET Core (Console Application)** — Application framework  
- **OpenAI API** — Embeddings and chat completions  
- **Qdrant** — Vector database for semantic search  
- **Semantic Kernel** — Agent orchestration and plugin support

---

## 🧪 Example Use Cases

- “What are today’s vegan options?”  
- “Recommend a dessert that pairs well with spicy food.”  
- “I’m allergic to nuts. What should I avoid?”

---

## 📦 Getting Started
Get started in 4 simple steps:
### 1. Get your API key:  
Go to GitHub > Settings > Developer Settings > Personal Access Tokens (or click on this [link](https://github.com/settings/tokens)), and generate a Classic API Key  
**or** use your own OpenAI API key (note: you'll need to update the base URL in the configuration if using OpenAI directly).

### 2. Clone the repository

Run the following commands:

```bash
git clone https://github.com/beheshty/Vectaurant.git
cd Vectaurant
```

### 3. Configure the API Key
Open the appsettings.json file inside the Vectaurant.Shared project and add your API key:

```json
{
  "OpenAIOptions": {
    "ApiKey": "your_openai_api_key_here"
}
```

### 4. Run the Projects

**First, run Qdrant on your local machine:**

```bash
docker run -p 6333:6333 qdrant/qdrant
```
Then in two separate terminals, run the following:

**Run the Kitchen project to feed the vector database:**
```bash
dotnet run --project ./Vectaurant.Kitchen/Vectaurant.Kitchen.csproj
```

**Then, run the FrontDesk project to start the AI agent:**

```bash
dotnet run --project ./Vectaurant.FrontDesk/Vectaurant.FrontDesk.csproj
```

---

## 🧩 Project Structure

- `Vectaurant.Shared` — Shared models and config
- `Vectaurant.Kitchen` — Feeds menu data into the vector database
- `Vectaurant.FrontDesk` — The AI agent that serves user requests

---

## 🤝 Contributing
Contributions are welcome! Feel free to open issues or submit pull requests to improve the project.
