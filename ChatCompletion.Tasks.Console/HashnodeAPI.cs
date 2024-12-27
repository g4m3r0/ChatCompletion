using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatCompletion.Tasks.Console
{
    public class HashnodeAPI
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string URL = "https://api.hashnode.com";
        private static readonly string HASHNODE_KEY = "aed995bc-14ef-44d4-8cd0-bb3d3e6b1d5f";

        public static async Task<string> CreatePost(string title, string contentMarkdown, string[] tags)
        {
            var query = @"
            mutation createStory($input: CreateStoryInput!) {
                createStory(input: $input) {
                    code
                    success
                    message
                }
            }
        ";

            var request = new
            {
                query = query,
                variables = new
                {
                    input = new
                    {
                        title = title,
                        contentMarkdown = contentMarkdown,
                        tags = tags
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(URL),
                Headers =
            {
                { HttpRequestHeader.Authorization.ToString(), HASHNODE_KEY },
            },
                Content = content
            };

            var response = await client.SendAsync(httpRequestMessage);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
