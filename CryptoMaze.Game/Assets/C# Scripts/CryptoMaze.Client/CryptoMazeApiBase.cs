using CryptoMaze.ClientServer.Authentication.Responses;
using Proyecto26;
using System;
using System.Data;
using System.IO;
using System.Security.Authentication;
using RSG;
using System.Collections.Generic;

namespace CryptoMaze.Client
{
    public abstract class CryptoMazeApiBase
    {
        public string ServerUrl { get; private set; }
        public CryptoMazeClientOptions Options { get; private set; }

        private readonly Func<IPromise<LoginResponse>> _refreshTokenFunc;

        public CryptoMazeApiBase(string serverUrl, CryptoMazeClientOptions options,
            Func<IPromise<LoginResponse>> refreshTokenFunc)
        {
            _refreshTokenFunc = refreshTokenFunc;

            ServerUrl = serverUrl;
            Options = options;
        }

        /// <exception cref="InvalidDataException">Options is null.</exception>
        /// <exception cref="InvalidCredentialException">Refresh token expired.</exception>
        /// <exception cref="InvalidExpressionException">RefreshTokenFunc is null.</exception>
        /// <exception cref="InvalidOperationException">Can't refresh access token.</exception>
        protected IPromise<T> Get<T>(string requestUrl, bool requireAuthorization = true, bool refreshNotRequired = false)
            where T : class
        {
            var updateTokenPromise = UpdateToken(requireAuthorization, refreshNotRequired);

            if (updateTokenPromise == null)
            {
                return RestClient.Get<T>(PrepareGetRequest(requestUrl, requireAuthorization));
            }
            else
            {
                return updateTokenPromise.Then(() =>
                {
                    return RestClient.Get<T>(PrepareGetRequest(requestUrl, requireAuthorization));
                });
            }
        }

        protected IPromise<T> Post<T>(string requestUrl, object data, bool requireAuthorization = true, bool refreshNotRequired = false)
            where T : class
        {
            var updateTokenPromise = UpdateToken(requireAuthorization, refreshNotRequired);

            if (updateTokenPromise == null)
            {
                return RestClient.Post<T>(PreparePostRequest(requestUrl, data, requireAuthorization));
            }
            else
            {
                return updateTokenPromise.Then(() =>
                {
                    return RestClient.Post<T>(PreparePostRequest(requestUrl, data, requireAuthorization));
                });
            }
        }

        protected IPromise<ResponseHelper> Post(string requestUrl, object data, bool requireAuthorization = true, bool refreshNotRequired = false)
        {
            var updateTokenPromise = UpdateToken(requireAuthorization, refreshNotRequired);

            if (updateTokenPromise == null)
            {
                return RestClient.Post(PreparePostRequest(requestUrl, data, requireAuthorization));
            }
            else
            {
                return updateTokenPromise.Then(() =>
                {
                    return RestClient.Post(PreparePostRequest(requestUrl, data, requireAuthorization));
                });
            }
        }

        private IPromise UpdateToken(bool requireAuthorization, bool refreshNotRequired)
        {
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

                        return _refreshTokenFunc()
                            .Then(result =>
                            {
                                Options.Update(result.accessToken, result.refreshToken,
                                    DateTime.UtcNow.AddSeconds(result.accessTokenExpireSeconds),
                                    DateTime.UtcNow.AddSeconds(result.refreshTokenExpireSeconds));
                            })
                            .Catch(err =>
                            {
                                throw new InvalidOperationException("Login required. Can't refresh access token.");
                            });
                    }
                }
            }

            return null;
        }

        private RequestHelper PrepareGetRequest(string uri, bool requireAuthorization)
        {
            var request = new RequestHelper();
            request.Uri = uri;

            if (requireAuthorization)
            {
                request.Headers = new Dictionary<string, string> {
                    { "Authorization", $"Bearer {Options.AccessToken}" }
                };
            }

            return request;
        }

        private RequestHelper PreparePostRequest(string uri, object data,
            bool requireAuthorization)
        {
            var request = new RequestHelper();
            request.Uri = uri;
            request.Body = data;

            if (requireAuthorization)
            {
                request.Headers = new Dictionary<string, string> {
                    { "Authorization", $"Bearer {Options.AccessToken}" }
                };
            }

            return request;
        }

        /*/// <exception cref="InvalidDataException">Options is null.</exception>
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
                    result = JsonUtility.FromJson<T>(stringResponse);
                }
                else
                {
                    error = new Error(response.StatusCode, stringResponse);
                }

                return new WebCallResult<T>(response.StatusCode, response.Headers, request.RequestUri.ToString(),
                    null, HttpMethod.Get, request.Headers, result, error);
            }
        }*/
    }
}