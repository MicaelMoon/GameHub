using Microsoft.AspNetCore.SignalR;

namespace GameHub
{
    public class GameHubServer : Hub
    {
        private static List<string> playerQueue = new List<string>();

        public async Task JoinQueue(string playerName)
        {
            playerQueue.Add(playerName);
            Console.WriteLine($"{playerName} joined the queue.");

            if (playerQueue.Count >= 2)
            {
                var player1 = playerQueue[0];
                var player2 = playerQueue[1];
                playerQueue.RemoveRange(0, 2);

                Console.WriteLine($"Match found: {player1} vs {player2}");

                // Notify both players that they are matched
                await Clients.Clients(player1, player2).SendAsync("MatchFound", player1, player2);

                // Start the game logic for the two players
                StartGame(player1, player2);
            }
            else
            {
                // Notify the player that they are waiting for an opponent
                await Clients.Caller.SendAsync("WaitingForOpponent");
            }
        }

        private void StartGame(string player1, string player2)
        {
            // Here you would start the game logic (you can spin up a new Docker container for this)
            Console.WriteLine($"Game starting between {player1} and {player2}...");
        }
    }
}
