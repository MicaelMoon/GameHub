using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    private static HubConnection _hubConnection;
    private static readonly List<string> _message = new();

    private static async Task Main(string[] args)
    {
        await GameHub();

        Console.WriteLine("In menu Exit");
        Console.ReadLine();
    }

    private static async Task GameHub()
    {
        try
        {
            string playerName = "Mario"; // Change to Player2 for the second instance

            // Setup the SignalR client connection
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7262/gameHub")
                .Build();

            connection.On<string, string>("MatchFound", (player1, player2) =>
            {
                Console.WriteLine($"Match found! {player1} vs {player2}");
            });

            connection.On("WaitingForOpponent", () =>
            {
                Console.WriteLine("Waiting for an opponent...");
            });

            await connection.StartAsync();

            // Join the queue
            await connection.InvokeAsync("JoinQueue", playerName);

            // Keep the console open
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static async Task ChatRoom()
    {
        _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7262/notifications").Build();

        _hubConnection.On<string>("RecievedNotification", message =>
        {
            _message.Add(message);
        });

        await _hubConnection.StartAsync();
    }
}
