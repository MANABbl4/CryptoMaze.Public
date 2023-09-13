using CryptoMaze.ClientServer.Authentication.Responses;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoMaze.Client
{
    public abstract class CryptoMazeApiBase
    {
        public Uri ServerUrl { get; private set; }
        public CryptoMazeClientOptions Options { get; private set; }

        private readonly Func<Task<WebCallResult<LoginResponse>>> _refreshTokenFunc;

        public CryptoMazeApiBase(string serverUrl, CryptoMazeClientOptions options,
            Func<Task<WebCallResult<LoginResponse>>> refreshTokenFunc)
        {
            _refreshTokenFunc = refreshTokenFunc;

            ServerUrl = new Uri(serverUrl);
            Options = options;
        }

        /// <exception cref="InvalidDataException">Options is null.</exception>
        /// <exception cref="InvalidCredentialException">Refresh token expired.</exception>
        /// <exception cref="InvalidExpressionException">RefreshTokenFunc is null.</exception>
        /// <exception cref="InvalidOperationException">Can't refresh access token.</exception>
        protected async Task<WebCallResult<T>> GetAsync<T>(Uri requestUrl, bool requireAuthorization = true, bool refreshNotRequired = false)
            where T : class
        {
            using (var client = new HttpClient())
            {
                var request = await PrepareRequest(HttpMethod.Get, requestUrl, string.Empty, requireAuthorization, refreshNotRequired);

                return await GetResponseAsync<T>(client, request);
            }
        }

        /// <exception cref="InvalidDataException">Options is null.</exception>
        /// <exception cref="InvalidCredentialException">Refresh token expired.</exception>
        /// <exception cref="InvalidExpressionException">RefreshTokenFunc is null.</exception>
        /// <exception cref="InvalidOperationException">Can't refresh access token.</exception>
        protected async Task<WebCallResult> PostAsync(Uri requestUrl, string jsonContent, bool requireAuthorization = true, bool refreshNotRequired = false)
        {
            using (var client = new HttpClient())
            {
                var request = await PrepareRequest(HttpMethod.Post, requestUrl, jsonContent, requireAuthorization, refreshNotRequired);

                return await GetResponseAsync(client, request);
            }
        }

        protected async Task<WebCallResult<T>> PostAsync<T>(Uri requestUrl, string jsonContent, bool requireAuthorization = true, bool refreshNotRequired = false)
            where T : class
        {
            using (var client = new HttpClient())
            {
                var request = await PrepareRequest(HttpMethod.Post, requestUrl, jsonContent, requireAuthorization, refreshNotRequired);

                return await GetResponseAsync<T>(client, request);
            }
        }

        /// <exception cref="InvalidDataException">Options is null.</exception>
        /// <exception cref="InvalidCredentialException">Refresh token expired.</exception>
        /// <exception cref="InvalidExpressionException">RefreshTokenFunc is null.</exception>
        /// <exception cref="InvalidOperationException">Can't refresh access token.</exception>
        private async Task<HttpRequestMessage> PrepareRequest(HttpMethod method, Uri requestUrl, string jsonContent,
            bool requireAuthorization, bool refreshNotRequired)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, requestUrl);
            request.Headers.Remove("Accept");
            request.Headers.Add("Accept", "application/json");

            if (requireAuthorization)
            {
                if (!refreshNotRequired)
                {
                    if (Options == null)
                    {
                        throw new InvalidDataException("Login required. Options is null.");
                    }

                    if (!Options.RefreshTokenExpirationDate.HasValue || Options.RefreshTokenExpirationDate.Value < DateTime.UtcNow)
                    {
                        throw new InvalidCredentialException("Login required. Refresh token expired.");
                    }
                    else if (!Options.AccessTokenExpirationDate.HasValue || Options.AccessTokenExpirationDate.Value < DateTime.UtcNow)
                    {
                        if (_refreshTokenFunc == null)
                        {
                            throw new InvalidExpressionException("Login required. RefreshTokenFunc is null.");
                        }

                        var tokenResult = await _refreshTokenFunc();

                        if (tokenResult.Success)
                        {
                            Options.Update(tokenResult.Data.AccessToken, tokenResult.Data.RefreshToken,
                                DateTime.UtcNow.AddSeconds(tokenResult.Data.AccessTokenExpireSeconds),
                                DateTime.UtcNow.AddSeconds(tokenResult.Data.RefreshTokenExpireSeconds));
                        }
                        else
                        {
                            throw new InvalidOperationException("Login required. Can't refresh access token.");
                        }
                    }
                }

                request.Headers.Add("Authorization", $"Bearer {Options.AccessToken}");
            }

            if (!string.IsNullOrEmpty(jsonContent))
            {
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private async Task<WebCallResult> GetResponseAsync(HttpClient client, HttpRequestMessage request)
        {
            using (HttpResponseMessage response = await client.SendAsync(request, CancellationToken.None))
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                Error error = null;

                if (!response.IsSuccessStatusCode)
                {
                    error = new Error(response.StatusCode, stringResponse);
                }

                return new WebCallResult(response.StatusCode, response.Headers, request.RequestUri.ToString(), null, HttpMethod.Get, request.Headers, error);
            }
        }

        private async Task<WebCallResult<T>> GetResponseAsync<T>(HttpClient client, HttpRequestMessage request)
            where T : class
        {
            using (HttpResponseMessage response = await client.SendAsync(request, CancellationToken.None))
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                T result = null;
                Error error = null;

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<T>(stringResponse);
                }
                else
                {
                    error = new Error(response.StatusCode, stringResponse);
                }

                return new WebCallResult<T>(response.StatusCode, response.Headers, request.RequestUri.ToString(),
                    null, HttpMethod.Get, request.Headers, result, error);
            }
        }
    }
}