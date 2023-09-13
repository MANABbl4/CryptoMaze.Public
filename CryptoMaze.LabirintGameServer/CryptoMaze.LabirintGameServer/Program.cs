using CryptoMaze.LabirintGameServer.BLL;
using CryptoMaze.LabirintGameServer.BLL.Services;
using CryptoMaze.LabirintGameServer.BLL.Utils;
using CryptoMaze.LabirintGameServer.DAL;
using CryptoMaze.LabirintGameServer.DAL.Entities;
using CryptoMaze.LabirintGameServer.DAL.Repositories;
using CryptoMaze.LabirintGameServer.Filters;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace CryptoMaze.LabirintGameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");

            // Add services to the container.
            builder.Services.AddScoped<ApplicationDbContext>();

            var basicConfig = builder.Configuration.GetSection("Basic")
                .Get<BasicConfiguration>(c => c.BindNonPublicProperties = true);

            var jwtConfig = builder.Configuration.GetSection("Jwt")
                .Get<JwtConfiguration>(c => c.BindNonPublicProperties = true);
            builder.Services.AddSingleton(jwtConfig);

            builder.Services.AddAuthentication()
                .AddJwtBearer(AuthenticationSchemes.JwtBearer, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
                    };
                })
                .AddJwtBearer(AuthenticationSchemes.JwtBearerNoLifeTime, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
                    };
                }).AddBasic(AuthenticationSchemes.Basic, options =>
                {
                    options.Realm = "My Application";
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidateCredentials = context =>
                        {
                            if (context.Username != null && context.Username.Equals(basicConfig.UserName) &&
                                context.Password != null && context.Password.Equals(basicConfig.Password))
                            {
                                var claims = new[]
                                {
                                    new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String),
                                    new Claim(ClaimTypes.Name, context.Username, ClaimValueTypes.String)
                                };

                                context.Principal = new ClaimsPrincipal(
                                    new ClaimsIdentity(claims, context.Scheme.Name));

                                context.Success();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            builder.Services.AddMemoryCache();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme },
                    BearerFormat = "JWT"
                });

                options.OperationFilter<AuthenticationRequirementsOperationFilter>();
            });

            builder.Logging.AddLog4Net();

            var gameConfig = builder.Configuration.GetSection("GameConfiguration")
                .Get<GameConfiguration>(c => c.BindNonPublicProperties = true);
            var leaderboardConfig = builder.Configuration.GetSection("LeaderboardConfiguration")
                .Get<LeaderboardConfiguration>(c => c.BindNonPublicProperties = true);

            builder.Services.AddSingleton(gameConfig);
            builder.Services.AddSingleton(leaderboardConfig);

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
            builder.Services.AddScoped<IShopProposalRepository, ShopProposalRepository>();
            builder.Services.AddScoped<IUserItemRepository, UserItemRepository>();
            builder.Services.AddScoped<IGenericRepository<Labirint, int>, GenericRepository<Labirint, int>>();
            builder.Services.AddScoped<IGenericRepository<LabirintCryptoBlock, Guid>, GenericRepository<LabirintCryptoBlock, Guid>>();
            builder.Services.AddScoped<IGenericRepository<LabirintEnergy, Guid>, GenericRepository<LabirintEnergy, Guid>>();
            builder.Services.AddScoped<IGenericRepository<LabirintCryptoKeyFragment, Guid>, GenericRepository<LabirintCryptoKeyFragment, Guid>>();
            builder.Services.AddScoped<IGenericRepository<SeasonHistory, int>, GenericRepository<SeasonHistory, int>>();
            builder.Services.AddScoped<IGenericRepository<ShopProposal, int>, GenericRepository<ShopProposal, int>>();
            builder.Services.AddScoped<IGenericRepository<Item, int>, GenericRepository<Item, int>>();

            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
            builder.Services.AddScoped<IShopService, ShopService>();

            var app = builder.Build();


            app.UseCors("AllowAllOrigins");

            var dbContext = new ApplicationDbContext(app.Configuration);
            dbContext.Database.Migrate();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}