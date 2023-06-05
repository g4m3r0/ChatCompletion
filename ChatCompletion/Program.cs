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
            while (true)
            {
                Console.Write("You: ");
                string prompt = Console.ReadLine();
                string response = await Common.ChatCompletion.CreateAsync(prompt);
                Console.WriteLine("ChatBot: " + response);
            }
        }
    }
}