using CryptoMaze.ClientServer.Authentication.Requests;
using CryptoMaze.ClientServer.Authentication.Responses;
using CryptoMaze.IdentityServer.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMaze.IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenController(ILogger<TokenController> logger, IRefreshTokenService refreshTokenService)
        {
            _logger = logger;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            try
            {
                var result = await _refreshTokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

                if (!result.Authorized)
                {
                    return BadRequest();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
