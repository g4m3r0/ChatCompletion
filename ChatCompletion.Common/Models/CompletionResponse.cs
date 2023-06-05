namespace ChatCompletion.Common.Models;

using System.Text.Json.Serialization;

public class CompletionResponse
{
    [JsonPropertyName("choices")]
    public Choice[] Choices { get; set; }
}
