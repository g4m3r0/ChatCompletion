namespace ChatCompletion.Common;

using global::ChatCompletion.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatCompletion
{
    public static async Task<string> CreateAsync(string prompt)
    {
        // Split prompt into chunks of 4096 tokens.
        var prompts = SplitPrompt(prompt);
        var totalResponse = new StringBuilder();

        foreach (var chunk in prompts)
        {
            try
            {
                var completion = await CompletionModel.CreateAsync(chunk);
                var completionData = JsonSerializer.Deserialize<CompletionResponse>(completion);
                var chatBotMessage = completionData?.Choices[0]?.Message?.Content ?? "No response found";

                totalResponse.Append(chatBotMessage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
                // If you want to stop processing on error, add a 'break' statement here.
            }
        }

        return totalResponse.ToString();
    }

    private static IEnumerable<string> SplitPrompt(string prompt)
    {
        int maxTokenSize = 2000;

        var words = prompt.Split(' ');
        var chunks = new List<string>();

        for (int i = 0; i < words.Length; i += maxTokenSize)
        {
            var chunkWords = words.Skip(i).Take(maxTokenSize);
            chunks.Add(string.Join(' ', chunkWords));
        }

        return chunks;
    }
}
