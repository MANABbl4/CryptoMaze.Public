using CryptoMaze.ClientServer.Authentication.Requests;
using CryptoMaze.ClientServer.Authentication.Responses;
using System.Threading.Tasks;

namespace CryptoMaze.Client
{
    public interface ICryptoMazeIdentityClient
    {
        Task<WebCallResult<SendCodeResponse>> SendCodeAsync(SendCodeRequest request);
        Task<WebCallResult<LoginResponse>> LoginAsync(LoginRequest request);
        Task<WebCallResult> LogoutAsync();
        Task<WebCallResult<LoginResponse>> RefreshTokenAsync();
    }
}
