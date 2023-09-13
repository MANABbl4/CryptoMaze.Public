using CryptoMaze.ClientServer.Authentication.Requests;
using CryptoMaze.ClientServer.Authentication.Responses;
using Proyecto26;
using RSG;

namespace CryptoMaze.Client
{
    public interface ICryptoMazeIdentityClient
    {
        CryptoMazeClientOptions Options { get; }
        IPromise<SendCodeResponse> SendCode(SendCodeRequest request);
        IPromise<LoginResponse> Login(LoginRequest request);
        IPromise<ResponseHelper> Logout();
        IPromise<LoginResponse> RefreshToken();
    }
}
