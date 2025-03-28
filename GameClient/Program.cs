using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    private static HubConnection connection;
    private static string playerName = "Mario"; 

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Press to enter looking for match");
        Console.ReadKey();
        await GameHub();

        Console.WriteLine("In menu Exit");
        Console.ReadLine();
    }

    private static async Task GameHub()
    {
        try
        {
            // Setup the SignalR client connection
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7262/gameHub")
                .Build();

            connection.On<string, string>("MatchFound", async (player1, player2) =>
            {
                Console.Clear();
                Console.WriteLine($"Match found! {player1} vs {player2}");
                await GameStart();
            });

            connection.On("WaitingForOpponent", () =>
            {
                Console.WriteLine("Waiting for an opponent...");
            });

            connection.On<string>("Results", async (winner) =>
            {
                Console.Clear();
                Console.WriteLine("Winner is " + winner);
            });

            connection.On("WaitingForOpponentToChoose", () =>
            {
                Console.WriteLine("Waiting for opponent to choose...");
            });

            await connection.StartAsync();

            // Join the queue
            await connection.InvokeAsync("JoinQueue", playerName);

            // Keep the console open
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static async Task GameStart()
    {
        try
        {
            Console.WriteLine("Rock, Paper, Scissor");
            string input = Console.ReadLine();
            await connection.InvokeAsync("Shoot", playerName, input);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
