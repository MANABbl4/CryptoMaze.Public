using CryptoMaze.ClientServer.Authentication.Responses;

namespace CryptoMaze.IdentityServer.BLL.Services
{
    public interface IRefreshTokenService
    {
        Task<LoginResponse> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
