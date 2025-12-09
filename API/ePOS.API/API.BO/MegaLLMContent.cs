using System.Collections.Generic;

namespace Web.BO
{
    public class ChatCompletionRequest
    {
        public string Model { get; set; }
        public List<Message> Messages { get; set; }
        public double? Temperature { get; set; }
        public int? MaxTokens { get; set; }
        public double? TopP { get; set; }
        public int? N { get; set; }
        public bool? Stream { get; set; }
        public List<string> Stop { get; set; }

        public ChatCompletionRequest(string model)
        {
            Model = model;
            Messages = new List<Message>();
        }

        public void AddMessage(string role, string content)
        {
            Messages.Add(new Message { Role = role, Content = content });
        }
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class ChatCompletionResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string FinishReason { get; set; }
    }

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}
