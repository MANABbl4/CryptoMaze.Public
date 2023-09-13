using CryptoMaze.TonPlayTelegramBot.Configurations;
using CryptoMaze.TonPlayTelegramBot.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoMaze.TonPlayTelegramBot.Controllers
{
    public class TelegramBotUpdateHandler
    {
        private const string BTN_GAME = "🎮 Game";
        private const string BTN_WEB_APP = "Web App";
        private const string MSG_START = "/start";

        private readonly ILogger<BotController> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly TelegramBotConfiguration _botConfig;
        private static APITonPlayTelegramLogin _apiTonPlayTelegramLogin;

        public TelegramBotUpdateHandler(ILogger<BotController> logger, TelegramBotConfiguration botConfig,
            ITelegramBotClient botClient)
        {
            _logger = logger;
            _botConfig = botConfig;
            _botClient = botClient;
            _apiTonPlayTelegramLogin = new APITonPlayTelegramLogin(_botConfig.TonPlayUrl);
        }

        public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                //This is the processing of any messages in a chat with a bot
                UpdateType.Message => BotOnMessageReceived(update.Message!),
                //This is what we use to handle when the user clicked on the play button.
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!),

                //This handles the event when the user has typed @botusername in the input field and should see popup
                //UpdateType.InlineQuery => GameBotOnInlineQueryReceived(botClient, update.InlineQuery!),
                //or you can use WebAppBotOnInlineQueryReceived instead of GameBotOnInlineQueryReceived
                UpdateType.InlineQuery => WebAppBotOnInlineQueryReceived(update.InlineQuery!),

                //There are other types of data in the telegram, but for example, we will process all of them in the same way
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
#pragma warning disable CA1031
            catch (Exception exception)
#pragma warning restore CA1031
            {
                await PollingErrorHandler(exception);
            }
        }

        private Task PollingErrorHandler(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        private async Task BotOnMessageReceived(Message message)
        {
            if (message.Text is not { } messageText)
                return;

            switch (messageText)
            {
                case MSG_START:
                    await HandleStart(message, true);
                    return;
                case BTN_GAME:
                    await HandleGame(message);
                    return;
                case BTN_WEB_APP:
                    await HandleWebApp(message);
                    return;
            }
            await HandleOtherMsgs(message);
        }

        private ReplyKeyboardMarkup GetMainMenuBtns()
        {

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { BTN_GAME, BTN_WEB_APP},
            })
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        private async Task HandleStart(Message message, bool withHello = false)
        {
            var chatId = message.Chat.Id;
            var username = message.From.Username;

            ReplyKeyboardMarkup replyKeyboardMarkup = GetMainMenuBtns();

            string msg = $"This is your game!";
            msg = withHello ? $"Hi, @{username}!\n" + msg : msg;

            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: msg,
                    replyMarkup: replyKeyboardMarkup);
        }

        private async Task HandleOtherMsgs(Message message, bool withHello = false)
        {
            var chatId = message.Chat.Id;

            ReplyKeyboardMarkup replyKeyboardMarkup = GetMainMenuBtns();

            string msg = $"🚀 Play right inside the bot!";

            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: msg,
                    replyMarkup: replyKeyboardMarkup);
        }

        private async Task HandleGame(Message message)
        {
            long chatId = message.Chat.Id;
            await _botClient.SendGameAsync(chatId, _botConfig.GameShortName);
        }

        //respond when the user clicked on the game button
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            string username = callbackQuery.From.Username;
            long userId = callbackQuery.From.Id;

            await _apiTonPlayTelegramLogin.PostTelegramUserLogin(
                callbackQuery.From.Id,
                callbackQuery.From.Username,
                callbackQuery.From.FirstName,
                callbackQuery.From.LastName,
                _botConfig.TonPlayBotKey,
                _botConfig.TonPlayGameKey,
                headerXAuthTonplay: _botConfig.XAuthTonPlay).ContinueWith(async (result) => {
                    string tokenString = "";
                    TokenModel token = JsonConvert.DeserializeObject<TokenModel>(result.Result);

                    if (result.IsFaulted)
                    {
                        Console.WriteLine($"Error receive token {callbackQuery.From.Username}: {result.Exception.Message}");
                        return;
                    }

                    tokenString = $"token={token.Token}";
                    string url = $"{_botConfig.GameUrl}?{tokenString}";
                    Console.WriteLine("url: " + url);
                    await _botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: callbackQuery.Id,
                        url: url);

                });
        }

        #region inline
        //simple example for game
        //request after click inline game btn
        private async Task GameBotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            InlineQueryResultGame inlineQueryResultGame = new InlineQueryResultGame(
                 id: "1",
                 gameShortName: _botConfig.GameShortName
            );

            InlineQueryResult[] results = {
            inlineQueryResultGame
        };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                   results: results,
                                                   isPersonal: true,
                                                   cacheTime: 0);

        }

        #endregion

        private async Task HandleWebApp(Message message)
        {
            string username = message.From.Username;
            long userId = message.From.Id;

            await _apiTonPlayTelegramLogin.PostTelegramUserLogin(
                message.From.Id,
                message.From.Username,
                message.From.FirstName,
                message.From.LastName,
                _botConfig.TonPlayBotKey,
                _botConfig.BotToken,
                headerXAuthTonplay: _botConfig.XAuthTonPlay).ContinueWith(async (result) => {

                    string tokenString = "";
                    TokenModel token = JsonConvert.DeserializeObject<TokenModel>(result.Result);

                    tokenString = $"token={token.Token}";

                    if (!result.IsFaulted && token != null && !string.IsNullOrWhiteSpace(token.Token))
                    {
                        tokenString = $"token={token.Token}";
                        Console.WriteLine(tokenString);
                    }
                    else if (result.IsFaulted)
                    {
                        Console.WriteLine($"Error receive token {message.From.Username}: {result.Exception.Message}");
                    }

                    WebAppInfo webAppInfo = new WebAppInfo();
                    webAppInfo.Url = $"{_botConfig.GameUrl}?{tokenString}";

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] {
                new KeyboardButton[] {
                    KeyboardButton.WithWebApp("Start game", webAppInfo)
                }
                    });

                    replyKeyboardMarkup.OneTimeKeyboard = true;

                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Click Start Game please", replyMarkup: replyKeyboardMarkup);
                });
        }

        #region inline
        //simple example for web app
        //request after click inline game btn
        private async Task WebAppBotOnInlineQueryReceived(InlineQuery inlineQuery)
        {

            InputMessageContent inputMessageContent = new InputTextMessageContent($"Let's play {_botConfig.GameShortName}");

            InlineQueryResult inlineQueryResult = new InlineQueryResultArticle(
                 id: "1",
                 title: "Ton Play game",
                 inputMessageContent: inputMessageContent
            );

            InlineQueryResult[] results = {
                inlineQueryResult
            };

            await _botClient.AnswerInlineQueryAsync(inlineQuery.Id,
                                                   results,
                                                   isPersonal: true,
                                                   cacheTime: 0);

        }
        #endregion
    }
}
