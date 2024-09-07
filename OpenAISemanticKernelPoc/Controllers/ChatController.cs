using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAISemanticKernelPoc.Models;
using System.Text.Json;

namespace OpenAISemanticKernelPoc.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatCompletionService _chatCompletionService;

        public ChatController(IChatCompletionService chatCompletionService)
        {
            _chatCompletionService = chatCompletionService;
        }

        public IActionResult Index()
        {
            var chatViewModel = new ChatViewModel
            {
                ChatHistory = GetChatHistory()
            };
            return View(chatViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserInput))
            {
                model.ChatHistory = GetChatHistory();
                return View("Index", model);
            }

            // Retrieve chat history from session
            var chatHistory = GetChatHistory();

            // Add user message to chat history
            chatHistory.Add(new ChatMessage { Role = "user", Content = model.UserInput });

            // Create a chat history object to store the conversation
            var history = new ChatHistory();
            history.AddUserMessage(model.UserInput);

            // Get the response from the AI
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings);

            // Add assistant message to chat history
            chatHistory.Add(new ChatMessage { Role = "assistant", Content = result.Content ?? string.Empty });

            // Save chat history back to session
            SaveChatHistory(chatHistory);

            // Update model with new chat history
            model.ChatHistory = chatHistory;

            return View("Index", model);
        }

        private List<ChatMessage> GetChatHistory()
        {
            var chatHistoryJson = HttpContext.Session.GetString("ChatHistory");
            if (string.IsNullOrEmpty(chatHistoryJson))
            {
                return new List<ChatMessage>();
            }
            return JsonSerializer.Deserialize<List<ChatMessage>>(chatHistoryJson);
        }

        private void SaveChatHistory(List<ChatMessage> chatHistory)
        {
            var chatHistoryJson = JsonSerializer.Serialize(chatHistory);
            HttpContext.Session.SetString("ChatHistory", chatHistoryJson);
        }
    }
}
