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
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JwtBearerNoLifeTime)]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameService _gameService;

        public GameController(ILogger<GameController> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        [HttpPost("StartGame")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StartGameResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StartGame()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.StartGameAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.StartGame failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("FinishGame")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinishGameResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FinishGame()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.FinishGameAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.FinishGame failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("StartLabirint")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StartLabirintResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StartLabirint()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.StartLabirintAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.StartLabirint failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("FinishLabirint")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinishLabirintResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FinishLabirint()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.FinishLabirintAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.FinishLabirint failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CollectCryptoBlock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CollectCryptoBlockResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CollectCryptoBlock(CollectCryptoBlockRequest request)
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.CollectCryptoBlockAsync(emailClaim.Value, request.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.CollectCryptoBlock failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CollectEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CollectEnergyResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CollectEnergy(CollectEnergyRequest request)
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.CollectEnergyAsync(emailClaim.Value, request.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.CollectEnergy failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CollectCryptoKeyFragment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CollectCryptoKeyFragmentResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CollectCryptoKeyFragment(CollectCryptoKeyFragmentRequest request)
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.CollectCryptoKeyFragmentAsync(emailClaim.Value, request.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.CollectCryptoKeyFragment failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("OpenStorage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenStorageResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> OpenStorage()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.OpenStorageAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.OpenStorage failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("UseSpeedRocketBooster")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UseSpeedRocketBoosterResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UseSpeedRocketBooster()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.UseSpeedRocketBoosterAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.UseSpeedRocketBooster failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("UseFreezeTimeBooster")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UseFreezeTimeBoosterResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UseFreezeTimeBooster()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _gameService.UseFreezeTimeBoosterAsync(emailClaim.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GameController.UseFreezeTimeBooster failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
