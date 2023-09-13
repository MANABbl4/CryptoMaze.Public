namespace CryptoMaze.Client
{
    public class CryptoMazeClient
    {
        public CryptoMazeClient(string identityServerUrl, string gameServerUrl, CryptoMazeClientOptions options)
        {
            Identity = new CryptoMazeIdentityApi(identityServerUrl, options);
            Game = new CryptoMazeLabirintGameApi(gameServerUrl, options, Identity.RefreshToken);
        }

        public ICryptoMazeIdentityClient Identity { get; private set; }
        public ICryptoMazeLabirintGameClient Game { get; private set; }
    }
}
