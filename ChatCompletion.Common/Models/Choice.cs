namespace ChatCompletion.Common.Models;

using System.Text.Json.Serialization;

public class Choice
{
    [JsonPropertyName("message")]
    public Message Message { get; set; }
}
