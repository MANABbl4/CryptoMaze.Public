using CryptoMaze.ClientServer.Authentication.Requests;
using CryptoMaze.ClientServer.Authentication.Responses;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CryptoMaze.Client
{
    public class CryptoMazeIdentityApi : CryptoMazeApiBase, ICryptoMazeIdentityClient
    {
        private readonly Uri _accountSendCodeUrl;
        private readonly Uri _accountLoginUrl;
        private readonly Uri _accountLogoutUrl;
        private readonly Uri _tokenRefreshUrl;

        public CryptoMazeIdentityApi(string serverUrl, CryptoMazeClientOptions options)
            : base(serverUrl, options, null)
        {
            _accountSendCodeUrl = new Uri(ServerUrl, new Uri("Account/SendCode", UriKind.Relative));
            _accountLoginUrl = new Uri(ServerUrl, new Uri("Account/Login", UriKind.Relative));
            _accountLogoutUrl = new Uri(ServerUrl, new Uri("Account/Logout", UriKind.Relative));
            _tokenRefreshUrl = new Uri(ServerUrl, new Uri("Token/Refresh", UriKind.Relative));
        }

        public Task<WebCallResult<SendCodeResponse>> SendCodeAsync(SendCodeRequest request)
        {
            return PostAsync<SendCodeResponse>(_accountSendCodeUrl, JsonConvert.SerializeObject(request), false);
        }

        public async Task<WebCallResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var result = await PostAsync<LoginResponse>(_accountLoginUrl, JsonConvert.SerializeObject(request), false);

            if (result.Success && result.Data.Authorized)
            {
                Options.Update(result.Data.AccessToken, result.Data.RefreshToken,
                    DateTime.UtcNow.AddSeconds(result.Data.AccessTokenExpireSeconds),
                    DateTime.UtcNow.AddSeconds(result.Data.RefreshTokenExpireSeconds));
            }

            return result;
        }

        public async Task<WebCallResult> LogoutAsync()
        {
            var result = await PostAsync(_accountLogoutUrl, string.Empty, true, true);

            if (result.Success)
            {
                Options.Update(string.Empty, string.Empty, null, null);
            }

            return result;
        }

        public async Task<WebCallResult<LoginResponse>> RefreshTokenAsync()
        {
            var request = new RefreshTokenRequest() { AccessToken = Options.AccessToken, RefreshToken = Options.RefreshToken };

            var result = await PostAsync<LoginResponse>(_tokenRefreshUrl, JsonConvert.SerializeObject(request), false);

            Options.Update(result.Data.AccessToken, result.Data.RefreshToken,
                    DateTime.UtcNow.AddSeconds(result.Data.AccessTokenExpireSeconds),
                    DateTime.UtcNow.AddSeconds(result.Data.RefreshTokenExpireSeconds));

            return result;
        }
    }
}
