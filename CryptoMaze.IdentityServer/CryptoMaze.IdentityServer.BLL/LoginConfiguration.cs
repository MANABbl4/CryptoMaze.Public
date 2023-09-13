namespace CryptoMaze.IdentityServer.BLL
{
    public class LoginConfiguration
    {
        public int LoginCodeMinWaitSeconds { get; private set; }
        public int LoginCodeExpiryHours { get; private set; }
        public int RefreshTokenExpiryDays { get; private set; }
    }
}
