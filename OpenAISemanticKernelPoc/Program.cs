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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sign-in users with the Microsoft identity platform
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApp(options => builder.Configuration.Bind("AzureAd", options));


// Add authorization with role-based policies
builder.Services.AddAuthorizationBuilder()
                                                 // Add authorization with role-based policies
                                                 .AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"))
                                                 // Add authorization with role-based policies
                                                 .AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));

builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

//In the Azure portal, the redirect URIs that you register on the Authentication page for your application need to match these URLs.
//    For the two preceding configuration files, they would be https://localhost:44359/signin-oidc.
////The reason is that applicationUrl is http://localhost:16011, but sslPort is specified (44359). CallbackPath is /signin-oidc, as defined in appsettings.json.
//In the same way, the sign-out URI would be set to https://localhost:44359/signout-oidc. In authentication section - under web add signin and in frontend signout add signout.
//Under Implicit grant and hybrid flows, select ID tokens.

// Add session services to the container
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

// Configure DbContext with SQL Server
//We use builder.Services.AddDbContext<ApplicationDbContext>() to register the ApplicationDbContext with the dependency injection system.
//UseSqlServer specifies that SQL Server (in this case, Azure SQL Database) is being used as the database provider.
//The connection string is retrieved from the application configuration.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));



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

app.UseAuthentication();

app.UseAuthorization();

// Use session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
