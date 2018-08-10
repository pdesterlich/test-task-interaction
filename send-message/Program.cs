using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace send_message
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hub")
                .Build();

            connection.On<string>("message", message => { Console.WriteLine($"signalr: {message}"); });

            connection.StartAsync().Wait();

            connection.InvokeAsync("SendMessage", "1234", "some message").Wait();
        }
    }
}
