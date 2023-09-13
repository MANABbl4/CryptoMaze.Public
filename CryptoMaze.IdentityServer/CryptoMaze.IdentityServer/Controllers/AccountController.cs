using CryptoMaze.ClientServer.Authentication.Requests;
using CryptoMaze.ClientServer.Authentication.Responses;
using CryptoMaze.IdentityServer.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMaze.IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("SendCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendCode(SendCodeRequest request)
        {
            try
            {
                var loginResult = await _accountService.SendCode(request.Email);

                return Ok(loginResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendCode failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _accountService.Login(request.Email, request.Code);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                if (authHeader.Count == 0 || string.IsNullOrEmpty(authHeader[0]))
                {
                    return Unauthorized();
                }

                await _accountService.Logout(authHeader[0].Split(' ')[1]);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}