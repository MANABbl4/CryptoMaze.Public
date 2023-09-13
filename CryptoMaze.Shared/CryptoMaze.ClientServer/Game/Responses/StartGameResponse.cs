namespace CryptoMaze.ClientServer.Game.Responses
{
    public class StartGameResponse
    {
        public bool Started { get; set; }
        public string Message { get; set; }
        public int EnergySpent { get; set; }
    }
}
