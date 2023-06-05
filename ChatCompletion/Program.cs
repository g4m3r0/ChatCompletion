using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System;

namespace ChatCompletion
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var chatBot = new ChatCompletion.Common.ChatCompletion();
            chatBot.Endpoint = "https://free.churchless.tech";
            //"https://chatgpt-api.shn.hk"

            while (true)
            {
                Console.Write("You: ");
                string prompt = Console.ReadLine();
<<<<<<< Updated upstream
                string response = await ChatCompletion.CreateAsync(prompt);
=======
                string response = await chatBot.CreateAsync(prompt);
>>>>>>> Stashed changes
                Console.WriteLine("ChatBot: " + response);
            }
        }
    }

    class CompletionModel
    {
        public static async Task<string> CreateAsync(string prompt)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "user", content = prompt }
            }
            };
            var json = JsonSerializer.Serialize(requestBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            Console.Write("Loading...");
            var response = await client.PostAsync("https://free.churchless.tech/v1/chat/completions", data);
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                return null;
            }
        }
    }

    class ChatCompletion
    {
        public static async Task<string> CreateAsync(string prompt)
        {
            var completion = await CompletionModel.CreateAsync(prompt);
            var completionData = JsonSerializer.Deserialize<CompletionResponse>(completion);
            var chatBotMessage = completionData?.Choices[0]?.Message?.Content ?? "No response found";

            return chatBotMessage;
        }
    }

    public class CompletionResponse
    {
        [JsonPropertyName("choices")]
        public Choice[] Choices { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}