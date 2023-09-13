namespace CryptoMaze.ClientServer.Authentication.Responses
{
    public class SendCodeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int NextRequestDelaySeconds { get; set; }
    }
}
