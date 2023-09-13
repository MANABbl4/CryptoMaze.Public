using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace CryptoMaze.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".data"] = "application/octet-stream";
            provider.Mappings[".wasm"] = "application/wasm";
            provider.Mappings[".br"] = "application/octet-stream";
            provider.Mappings[".js"] = "application/javascript";

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Game")),
                RequestPath = "/Game",
                ContentTypeProvider = provider,
                OnPrepareResponse = context =>
                {
                    var path = context.Context.Request.Path.Value;
                    var extension = Path.GetExtension(path);

                    if (extension == ".gz" || extension == ".br")
                    {
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path) ?? "";
                        if (provider.TryGetContentType(fileNameWithoutExtension, out string? contentType))
                        {
                            context.Context.Response.ContentType = contentType;
                            context.Context.Response.Headers.Add("Content-Encoding", extension == ".gz" ? "gzip" : "br");
                        }
                    }
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}