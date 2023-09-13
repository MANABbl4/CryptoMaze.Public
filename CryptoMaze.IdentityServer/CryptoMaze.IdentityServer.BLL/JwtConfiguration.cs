namespace CryptoMaze.IdentityServer.BLL
{
    public class JwtConfiguration
    {
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public string SecretKey { get; private set; }
        public int ExpireSeconds { get; private set; }
    }
}
