using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.LabirintGameServer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoMaze.LabirintGameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JwtBearer)]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerService _playerService;

        public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
        }

        [HttpGet("GetData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerDataResponse))]
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

                return Ok(await _playerService.GetDataAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"PlayerController.GetData failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("SetName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SetNameResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetName(SetNameRequest request)
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _playerService.SetNameAsync(emailClaim.Value, request.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"PlayerController.SetName with Name={request.Name} failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("AddEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddEnergyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEnergy()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _playerService.AddEnergyAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"PlayerController.AddEnergy failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}