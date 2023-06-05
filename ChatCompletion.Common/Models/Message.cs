namespace ChatCompletion.Common.Models;

using System.Text.Json.Serialization;

public class Message
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
