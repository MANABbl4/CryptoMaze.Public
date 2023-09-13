namespace CryptoMaze.ClientServer.Authentication.Responses
{
    public class SendCodeResponse
    {
        public bool success;
        public string message;
        public int nextRequestDelaySeconds;
    }
}
