using CryptoMaze.TonPlayTelegramBot.Configurations;
using Telegram.Bot.Polling;
using Telegram.Bot;
using CryptoMaze.TonPlayTelegramBot.Controllers;

namespace CryptoMaze.TonPlayTelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");

            // Add services to the container.
            var telegramBotConfig = builder.Configuration
                .GetSection("TelegramBotConfiguration")
                .Get<TelegramBotConfiguration>(c => c.BindNonPublicProperties = true);
            builder.Services.AddSingleton(telegramBotConfig);

            // Register named HttpClient to get benefits of IHttpClientFactory
            // and consume it with ITelegramBotClient typed client.
            // More read:
            //  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#typed-clients
            //  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            builder.Services
                .AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotClientOptions options = new(telegramBotConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

            // Dummy business-logic service
            builder.Services.AddScoped<TelegramBotUpdateHandler>();

            // There are several strategies for completing asynchronous tasks during startup.
            // Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
            // We are going to use IHostedService to add and later remove Webhook
            builder.Services.AddHostedService<WebhookService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}