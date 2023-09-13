using Microsoft.Extensions.Options;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using CryptoMaze.TonPlayTelegramBot.Configurations;

namespace CryptoMaze.TonPlayTelegramBot.Controllers
{
    public class WebhookService : IHostedService
    {
        private readonly ILogger<WebhookService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TelegramBotConfiguration _botConfig;

        public WebhookService(
            ILogger<WebhookService> logger,
            IServiceProvider serviceProvider,
            IOptions<TelegramBotConfiguration> botOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _botConfig = botOptions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Configure custom endpoint per Telegram API recommendations:
            // https://core.telegram.org/bots/api#setwebhook
            // If you'd like to make sure that the webhook was set by you, you can specify secret data
            // in the parameter secret_token. If specified, the request will contain a header
            // "X-Telegram-Bot-Api-Secret-Token" with the secret token as content.
            var webhookAddress = $"{_botConfig.HostAddress}{_botConfig.Route}";
            _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                secretToken: _botConfig.SecretToken,
                cancellationToken: cancellationToken);
            /*botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: PollingErrorHandler,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationToken);*/

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Remove webhook on app shutdown
            _logger.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
