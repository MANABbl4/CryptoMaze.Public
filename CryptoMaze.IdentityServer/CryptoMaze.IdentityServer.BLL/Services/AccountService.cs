using Azure.Core;
using CryptoMaze.ClientServer.Authentication.Responses;
using CryptoMaze.IdentityServer.DAL.Entities;
using CryptoMaze.IdentityServer.DAL.Repositories;
using EmailService;
using System.Security.Claims;

namespace CryptoMaze.IdentityServer.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginCodeRepository _userLoginCodeRepository;
        private readonly IEmailSender _emailSender;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly EmailsConfiguration _emailsConfiguration;
        private readonly JwtConfiguration _jwtConfiguration;

        public AccountService(ITokenService tokenService, IUserRepository userRepository,
            IUserLoginCodeRepository userLoginCodeRepository, IEmailSender emailSender,
            LoginConfiguration loginConfiguration, EmailsConfiguration emailsConfiguration, JwtConfiguration jwtConfiguration)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _userLoginCodeRepository = userLoginCodeRepository;
            _emailSender = emailSender;
            _loginConfiguration = loginConfiguration;
            _emailsConfiguration = emailsConfiguration;
            _jwtConfiguration = jwtConfiguration;
        }

        public async Task<SendCodeResponse> SendCode(string email)
        {
            var lastLogin = await _userLoginCodeRepository.GetLastByUserEmailAsync(email);

            if (lastLogin != null)
            {
                var delay = _loginConfiguration.LoginCodeMinWaitSeconds - (int)(DateTime.UtcNow - lastLogin.Created).TotalSeconds;

                if (delay > 0)
                {
                    return new SendCodeResponse()
                    {
                        Success = false,
                        Message = $"Please, wait for {delay} seconds before next request.",
                        NextRequestDelaySeconds = delay
                    };
                }
            }

            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User()
                {
                    Email = email,
                    RegisteredDate = DateTime.UtcNow,
                };

                await _userRepository.AddAsync(user);
            }

            var random = new Random();
            var code = random.Next(0, 999999);
            var loginCode = code.ToString("D6");

            await _userLoginCodeRepository.AddAsync(new UserLoginCode()
            {
                Code = loginCode,
                Created = DateTime.UtcNow,
                Expiry = DateTime.UtcNow.AddHours(_loginConfiguration.LoginCodeExpiryHours),
                UserId = user.Id,
                Used = false
            });

            await _emailSender.SendEmailAsync(new Message(new string[] { email },
                _emailsConfiguration.LoginCode.Subject.Replace("[Code]", loginCode),
                _emailsConfiguration.LoginCode.Content.Replace("[Code]", loginCode).Replace("[LoginCodeExpiryHours]", _loginConfiguration.LoginCodeExpiryHours.ToString()),
                null));

            return new SendCodeResponse()
            {
                Success = true,
                Message = $"Message was send to {email}.",
                NextRequestDelaySeconds = _loginConfiguration.LoginCodeMinWaitSeconds
            };
        }

        public async Task<LoginResponse> Login(string email, string code)
        {
            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null )
            {
                return new LoginResponse() { Authorized = false };
            }

            var lastLogin = await _userLoginCodeRepository.GetLastByUserEmailAsync(email);

            if (lastLogin == null || lastLogin.Used || !lastLogin.Code.Equals(code))
            {
                return new LoginResponse() { Authorized = false };
            }

            lastLogin.Used = true;
            await _userLoginCodeRepository.UpdateAsync(lastLogin);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(_loginConfiguration.RefreshTokenExpiryDays);

            await _userRepository.UpdateAsync(user);

            return new LoginResponse()
            {
                Authorized = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpireSeconds = _jwtConfiguration.ExpireSeconds,
                RefreshTokenExpireSeconds = _loginConfiguration.RefreshTokenExpiryDays * 86400
            };
        }

        public async Task Logout(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);

            var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return;
            }

            var user = await _userRepository.FindByEmailAsync(emailClaim.Value);

            if (user == null)
            {
                return;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiration = null;

            await _userRepository.UpdateAsync(user);
        }
    }
}
