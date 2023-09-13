using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.LabirintGameServer.BLL.Models;
using CryptoMaze.LabirintGameServer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CryptoMaze.LabirintGameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILogger<LeaderboardController> _logger;
        private readonly ILeaderboardService _leaderboardService;
        private readonly IMemoryCache _memoryCache;

        private const string _leaderboardDataCacheKey = "leaderboardData";

        public LeaderboardController(ILogger<LeaderboardController> logger, ILeaderboardService leaderboardService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _leaderboardService = leaderboardService;
            _memoryCache = memoryCache;
        }

        [HttpGet("GetData")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.JwtBearer)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeaderboardDataResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                var leaderboardData = await _memoryCache.GetOrCreateAsync(_leaderboardDataCacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

                    return await _leaderboardService.GetCurrentSeasonDataAsync();
                });

                return Ok(await _leaderboardService.GetDataAsync(emailClaim.Value, leaderboardData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"LeaderboardController.GetData failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("FinishSeason")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Basic)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinishSeasonResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FinishSeason()
        {
            try
            {
                return Ok(await _leaderboardService.FinishSeasonAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"LeaderboardController.FinishSeason failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
