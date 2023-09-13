namespace CryptoMaze.ClientServer.Authentication.Responses
{
    public class LoginResponse
    {
        public bool authorized;
        public string accessToken;
        public string refreshToken;
        public int accessTokenExpireSeconds;
        public int refreshTokenExpireSeconds;
    }
}
