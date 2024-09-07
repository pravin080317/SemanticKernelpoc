namespace OpenAISemanticKernelPoc.Models
{
    public class ChatViewModel
    {
        public string UserInput { get; set; }
        public List<ChatMessage> ChatHistory { get; set; } = new List<ChatMessage>();
    }
}
