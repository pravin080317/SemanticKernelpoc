using Azure.AI.OpenAI;
using Azure;
using Azure.Identity;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using OpenAISemanticKernelPoc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add semantic kernel services 

// Load configuration from environment variables or appsettings
var useOpenAI = builder.Configuration.GetValue<bool>("UseOpenAI", true);  // Default to true if not specified
var useManagedIdentity = builder.Configuration.GetValue<bool>("UseManagedIdentity", false);  // Default to false if not specified

var openAIEndpoint = Environment.GetEnvironmentVariable("AZURE_AI_ENDPOINT")
                     ?? builder.Configuration.GetValue<string>("AzureOpenAI:Endpoint");
var apiKey = Environment.GetEnvironmentVariable("AZURE_AI_KEY")
             ?? builder.Configuration.GetValue<string>("AzureOpenAI:ApiKey");

// Configure OpenAI or Azure OpenAI client
OpenAIClient azureOpenAIClient = useManagedIdentity
    ? new OpenAIClient(new Uri(openAIEndpoint), new DefaultAzureCredential())
    : new OpenAIClient(new Uri(openAIEndpoint), new Azure.AzureKeyCredential(apiKey));

// Add Semantic Kernel services based on the configuration
if (useOpenAI)
{
    builder.Services.AddSingleton<IChatCompletionService>(new AzureOpenAIChatCompletionService("gpt-4", azureOpenAIClient));
    //builder.Services.AddSingleton<ITextEmbeddingGenerationService>(new AzureOpenAITextEmbeddingGenerationService("embeddingsmall", azureOpenAIClient));
}
//else
//{
//    var endpoint = builder.Configuration.GetValue<string>("Ollama:Endpoint", "http://localhost:11434/");
//    builder.Services.AddSingleton<IChatCompletionService>(new OllamaChatCompletionService("llama3.1", endpoint));
//    builder.Services.AddSingleton<ITextEmbeddingGenerationService>(new OllamaTextEmbeddingGenerationService("all-minilm", new Uri(endpoint)));
//}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
