using CryptoMaze.ClientServer.Authentication.Responses;

namespace CryptoMaze.IdentityServer.BLL.Services
{
    public interface IAccountService
    {
        Task<SendCodeResponse> SendCode(string email);
        Task<LoginResponse> Login(string email, string code);
        Task Logout(string token);
    }
}
