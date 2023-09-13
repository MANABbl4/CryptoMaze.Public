using System;

namespace CryptoMaze.Client
{
    public class CryptoMazeClientOptions
    {
        public string AccessToken { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;
        public DateTime? AccessTokenExpirationDate { get; private set; } = null;
        public DateTime? RefreshTokenExpirationDate { get; private set; } = null;

        public CryptoMazeClientOptions(string accessToken, string refreshToken,
            DateTime? accessTokenExpirationDate, DateTime? refreshTokenExpirationDate)
        {
            Update(accessToken, refreshToken, accessTokenExpirationDate, refreshTokenExpirationDate);
        }

        public void Update(string accessToken, string refreshToken,
            DateTime? accessTokenExpirationDate, DateTime? refreshTokenExpirationDate)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpirationDate = accessTokenExpirationDate;
            RefreshTokenExpirationDate = refreshTokenExpirationDate;
        }
    }
}
