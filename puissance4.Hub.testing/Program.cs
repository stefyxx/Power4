using Microsoft.AspNetCore.SignalR.Client;

namespace puissance4.Hub.testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //INSTALL package: Microsoft.AspNetCore.SignalR.Client

            //HubConnection connection = new HubConnection().WithUrl();
            HubConnectionBuilder builder = new HubConnectionBuilder();
            HubConnection connection = builder
                .WithUrl("https://puissance4.azurewebsites.net/ws/game")  // Questo mi permette di connettermi al mio 'GameHub' into API --> into program.cs: app.MapHub<GameHub>("/ws/game");

                .WithAutomaticReconnect()
                .Build();
            //Start Connection to websocket
            connection.StartAsync().Wait();

            //ascolto in permanenza
            connection.On<string>("OnMessage", m => { Console.WriteLine(m); });
            //"OnMessage" into GameHub 

            while (true)
            {
                //Console.ReadKey();  //qualsiasi cosa tocco, lancio il method 'SayHello' di GameHub into API
                //connection.SendAsync("SayHello");

                string m = Console.ReadLine(); // invio un sms
                connection?.SendAsync("SayHello", m);
            }
        }
    }
}
