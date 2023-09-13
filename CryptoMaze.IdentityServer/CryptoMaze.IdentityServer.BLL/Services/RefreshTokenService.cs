using CryptoMaze.ClientServer.Authentication.Responses;
using CryptoMaze.IdentityServer.DAL.Repositories;
using System.Security.Claims;

namespace CryptoMaze.IdentityServer.BLL.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly JwtConfiguration _jwtConfiguration;

        public RefreshTokenService(ITokenService tokenService, IUserRepository userRepository,
            LoginConfiguration loginConfiguration, JwtConfiguration jwtConfiguration)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _loginConfiguration = loginConfiguration;
            _jwtConfiguration = jwtConfiguration;
        }

        public async Task<LoginResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return new LoginResponse() { Authorized = false };
            }

            var user = await _userRepository.FindByEmailAsync(emailClaim.Value);

            if (user == null)
            {
                return new LoginResponse() { Authorized = false };
            }

            if (!user.RefreshToken.Equals(refreshToken) || user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return new LoginResponse() { Authorized = false };
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(_loginConfiguration.RefreshTokenExpiryDays);

            await _userRepository.UpdateAsync(user);

            return new LoginResponse()
            {
                Authorized = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpireSeconds = _jwtConfiguration.ExpireSeconds,
                RefreshTokenExpireSeconds = _loginConfiguration.RefreshTokenExpiryDays * 86400
            };
        }
    }
}
