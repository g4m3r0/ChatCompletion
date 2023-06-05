﻿namespace ChatCompletion.Common;

using global::ChatCompletion.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatCompletion
{
    public string Endpoint { get; set; } = "https://free.churchless.tech";
    //public string Endpoint { get; set; } = ""https://chatgpt-api.shn.hk"

    public async Task<string> CreateAsync(string prompt, string context)
    {
        // Split prompt into chunks of 4096 tokens.
        var prompts = SplitPrompt($"Prompt: {prompt}", $"Context: {context}").ToArray();
        var totalResponse = new StringBuilder();

        for (var i = 0; i < prompts.Count(); i++)
        {
            try
            {
                var chunk = prompts[i];
                var completion = await CompletionModel.CreateAsync(this.Endpoint, chunk);
                var completionData = JsonSerializer.Deserialize<CompletionResponse>(completion);
                var chatBotMessage = completionData?.Choices[0]?.Message?.Content ?? "No response found";

                //totalResponse.Append($"Response for Chunk {i}:");
                totalResponse.Append(chatBotMessage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        return totalResponse.ToString();
    }

    public async Task<string> CreateAsync(string prompt)
    {
        try
        {
            var completion = await CompletionModel.CreateAsync(this.Endpoint, prompt);
            var completionData = JsonSerializer.Deserialize<CompletionResponse>(completion);
            var chatBotMessage = completionData?.Choices[0]?.Message?.Content ?? "No response found";
            return chatBotMessage;
        }
        catch (HttpRequestException e)
        {
            return $"An error occurred: {e.Message}";
        }
    }

    private static IEnumerable<string> SplitPrompt(string prompt, string context)
    {

        var words = context.Split(' ');
        var promptWords = prompt.Split(' ');

        var maxTokenSize = 2000 - promptWords.Count();
        var chunks = new List<string>();

        for (int i = 0; i < words.Length; i += maxTokenSize)
        {
            var chunkWords = words.Skip(i).Take(maxTokenSize);

            chunks.Add(string.Join(' ', promptWords));
            chunks.Add(string.Join(' ', chunkWords));
        }

        return chunks;
    }
}