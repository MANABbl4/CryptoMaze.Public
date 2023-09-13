using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.LabirintGameServer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CryptoMaze.LabirintGameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JwtBearer)]
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly IShopService _shopService;
        private readonly IMemoryCache _memoryCache;

        private const string _shopProposalsDataCacheKey = "shopProposalsData";

        public ShopController(ILogger<ShopController> logger, IShopService shopService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _shopService = shopService;
            _memoryCache = memoryCache;
        }

        [HttpGet("GetData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShopDataResponse))]
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

                var shopProposalsData = await _memoryCache.GetOrCreateAsync(_shopProposalsDataCacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                    return await _shopService.GetDataAsync();
                });

                return Ok(shopProposalsData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ShopController.GetData failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("Buy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Buy(BuyRequest request)
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized();
                }

                return Ok(await _shopService.BuyAsync(emailClaim.Value, request.ProposalId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ShopController.AddEnergy failed with exception.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
