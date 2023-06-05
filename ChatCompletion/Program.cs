using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System;
using ChatCompletion.Common;

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
                string response = await chatBot.CreateAsync(prompt);
                Console.WriteLine("ChatBot: " + response);
            }
        }
    }
}