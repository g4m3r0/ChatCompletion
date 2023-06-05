﻿namespace ChatCompletion.Common.Models;

using System;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

public class CompletionModel
{
    public static async Task<string> CreateAsync(string endpoint, string prompt)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
                {
                new { role = "user", content = prompt }
                }
        };
        var json = JsonSerializer.Serialize(requestBody);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        Console.Write("Loading...");
        var response = await client.PostAsync($"{endpoint}/v1/chat/completions", data);
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        else
        {
            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }
    }
}