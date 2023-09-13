using CryptoMaze.TonPlayTelegramBot.Filters;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CryptoMaze.TonPlayTelegramBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly TelegramBotUpdateHandler _updateHandlerService;

        public BotController(ILogger<BotController> logger, TelegramBotUpdateHandler updateHandlerService)
        {
            _logger = logger;
            _updateHandlerService = updateHandlerService;
        }

        [HttpPost]
        [ValidateTelegramBot]
        public async Task<IActionResult> Post([FromBody] Update update,
            [FromServices] TelegramBotUpdateHandler updateHandler,
            CancellationToken cancellationToken)
        {
            await _updateHandlerService.HandleUpdateAsync(update, cancellationToken);
            return Ok();
        }
    }
}