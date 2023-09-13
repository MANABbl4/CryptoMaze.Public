using CryptoMaze.ClientServer.Authentication.Requests;
using CryptoMaze.ClientServer.Authentication.Responses;
using Proyecto26;
using RSG;
using System;

namespace CryptoMaze.Client
{
    public class CryptoMazeIdentityApi : CryptoMazeApiBase, ICryptoMazeIdentityClient
    {
        private readonly string _accountSendCodeUrl;
        private readonly string _accountLoginUrl;
        private readonly string _accountLogoutUrl;
        private readonly string _tokenRefreshUrl;

        public CryptoMazeIdentityApi(string serverUrl, CryptoMazeClientOptions options)
            : base(serverUrl, options, null)
        {
            _accountSendCodeUrl = ServerUrl + "/Account/SendCode";
            _accountLoginUrl = ServerUrl + "/Account/Login";
            _accountLogoutUrl = ServerUrl + "/Account/Logout";
            _tokenRefreshUrl = ServerUrl + "/Token/Refresh";
        }

        public IPromise<SendCodeResponse> SendCode(SendCodeRequest request)
        {
            return Post<SendCodeResponse>(_accountSendCodeUrl, request, false);
        }

        public IPromise<LoginResponse> Login(LoginRequest request)
        {
            return Post<LoginResponse>(_accountLoginUrl, request, false).Then(result =>
            {
                if (result.authorized)
                {
                    Options.Update(result.accessToken, result.refreshToken,
                        DateTime.UtcNow.AddSeconds(result.accessTokenExpireSeconds),
                        DateTime.UtcNow.AddSeconds(result.refreshTokenExpireSeconds));
                }

                return result;
            });
        }

        public IPromise<ResponseHelper> Logout()
        {
            return Post(_accountLogoutUrl, null, true, true).Then(result =>
            {
                Options.Update(string.Empty, string.Empty, null, null);

                return result;
            });
        }

        public IPromise<LoginResponse> RefreshToken()
        {
            var request = new RefreshTokenRequest() { accessToken = Options.AccessToken, refreshToken = Options.RefreshToken };

            return Post<LoginResponse>(_tokenRefreshUrl, request, false).Then(result =>
            {
                if (result.authorized)
                {
                    Options.Update(result.accessToken, result.refreshToken,
                        DateTime.UtcNow.AddSeconds(result.accessTokenExpireSeconds),
                        DateTime.UtcNow.AddSeconds(result.refreshTokenExpireSeconds));
                }

                return result;
            });
        }
    }
}
