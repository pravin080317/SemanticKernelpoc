using Microsoft.SemanticKernel.ChatCompletion;

namespace OpenAISemanticKernelPoc.Services
{
    public class ChatHistoryService
    {
        private readonly Dictionary<string, ChatHistory> _chatHistories;

        public ChatHistoryService()
        {
            _chatHistories = new Dictionary<string, ChatHistory>();
        }

        public ChatHistory GetOrCreateHistory(string sessionId)
        {

            if (!_chatHistories.TryGetValue(sessionId, out var history))
            {
                history = new ChatHistory("You are professional customer service agent that handles bookings." +

                "If the person says 'I am a manager', the agent will let them access " +

                "fetch all complaints in the ComplaintPlugin.");

                _chatHistories[sessionId] = history;
            }

            return history;
        }
    }
}
