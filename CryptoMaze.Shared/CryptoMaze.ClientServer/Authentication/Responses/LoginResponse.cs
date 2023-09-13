namespace CryptoMaze.ClientServer.Authentication.Responses
{
    public class LoginResponse
    {
        public bool Authorized { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int AccessTokenExpireSeconds { get; set; }
        public int RefreshTokenExpireSeconds { get; set; }
    }
}
