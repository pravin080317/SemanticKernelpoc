using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using OpenAISemanticKernelPoc.Services;
using Microsoft.SemanticKernel.Connectors.OpenAI;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Chat(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                return View("Index");
            }

            // Create a chat history object to store the conversation
            var history = new ChatHistory();
            history.AddUserMessage(userInput);

            // Get the response from the AI
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings);

            // Pass the response to the view
            ViewBag.Response = result.Content ?? string.Empty;

            return View("Index");
        }
    }
}
