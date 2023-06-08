namespace ChatCompletion.Tasks.Console;

using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System;
using ChatCompletion.Common;
using YoutubeTranscriptApi;

public class Program
{
    private static readonly int MaxChunkLength = 2000;
    private static readonly ChatCompletion chatBot = new ChatCompletion();

    /// <summary>
    /// Main entry point of the program.
    /// </summary>
    static async Task Main(string[] args)
    {
        var transcript = await GetYoutubeTranscript("X6NCRYvNKFw");
        Console.WriteLine($"Transcript: {transcript}");

        // Process the transcript
        var chunks = SliceTextIntoChunks(transcript, MaxChunkLength);
        var summarizedChunks = await SummarizeChunks(chunks);
        var keyPoints = await ExtractKeyPointsFromSummaries(summarizedChunks);

        await File.WriteAllLinesAsync("keynote.txt", keyPoints);
        await CreateBlogPostFromKeyPoints(keyPoints);
    }

    /// <summary>
    /// Slices the input text into chunks of a specified size.
    /// </summary>
    private static List<string> SliceTextIntoChunks(string text, int maxChunkLength)
    {
        var chunks = new List<string>();
        for (int i = 0; i < text.Length; i += maxChunkLength)
        {
            chunks.Add(text.Substring(i, Math.Min(maxChunkLength, text.Length - i)));
        }
        return chunks;
    }

    /// <summary>
    /// Summarizes each chunk of text using the chat bot.
    /// </summary>
    private static async Task<List<string>> SummarizeChunks(List<string> chunks)
    {
        var summarizedChunks = new List<string>();
        foreach (var chunk in chunks)
        {
            var prompt = $"Provide a concise summary of this passage. The text should be written in the style of wikipedia. Only output the summarized text: {chunk}";
            var response = await chatBot.CreateAsync(prompt);
            Console.WriteLine(response);
            summarizedChunks.Add(response);
        }
        return summarizedChunks;
    }

    /// <summary>
    /// Extracts key points from each summary using the chat bot.
    /// </summary>
    private static async Task<List<string>> ExtractKeyPointsFromSummaries(List<string> summaries)
    {
        var keyPoints = new List<string>();
        foreach (var summary in summaries)
        {
            var prompt = $"Identify the key points or takeaways in this summary: {summary}";
            var response = await chatBot.CreateAsync(prompt);
            Console.WriteLine(response);
            keyPoints.Add(response);
        }
        return keyPoints;
    }

    /// <summary>
    /// Creates a blog post from the key points using the chat bot.
    /// </summary>
    private static async Task CreateBlogPostFromKeyPoints(List<string> keyPoints)
    {
        foreach (var keyPoint in keyPoints)
        {
            var prompt = $"Expand this key points into a full paragraph suitable for a blog post. Format it using markdown: {keyPoint}";
            var response = await chatBot.CreateAsync(prompt);
            await File.AppendAllTextAsync("blogpost.txt", response);
            Console.WriteLine(response);
        }
    }

    /// <summary>
    /// Fetches a YouTube video's transcript.
    /// </summary>
    public static async Task<string> GetYoutubeTranscript(string videoId)
    {
        using (var youTubeTranscriptApi = new YouTubeTranscriptApi())
        {
            var transcriptItems = youTubeTranscriptApi.GetTranscript(videoId);
            var transcript = string.Join(" ", transcriptItems.Select(x => x.Text));
            return transcript;
        }
    }
}