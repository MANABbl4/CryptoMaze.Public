using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CryptoMaze.IdentityServer.Filters;
using System.Text;
using CryptoMaze.IdentityServer.DAL;
using CryptoMaze.IdentityServer.DAL.Repositories;
using EmailService;
using CryptoMaze.IdentityServer.BLL;
using CryptoMaze.IdentityServer.BLL.Services;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");

            // Add services to the container.
            builder.Services.AddScoped<ApplicationDbContext>();

            var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>(c => c.BindNonPublicProperties = true);
            builder.Services.AddSingleton(jwtConfig);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
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

            var loginConfig = builder.Configuration
                .GetSection("LoginConfiguration")
                .Get<LoginConfiguration>(c => c.BindNonPublicProperties = true);

            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>(c => c.BindNonPublicProperties = true);

            var emails = builder.Configuration
                .GetSection("Emails")
                .Get<EmailsConfiguration>(c => c.BindNonPublicProperties = true);

            builder.Services.AddSingleton(loginConfig);
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddSingleton(emails);

            builder.Services.AddScoped<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserLoginCodeRepository, UserLoginCodeRepository>();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

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