namespace GameGameModels
{
    public class Player(string id, string name)
    {
        public string Id { get; set; } = id;
        public string PlayerName { get; set; } = name;

    }
}
