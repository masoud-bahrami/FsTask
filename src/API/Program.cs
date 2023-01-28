using FsTask.Bootstrapper;
using FsTask.QuestDB;
using Microsoft.Extensions.Configuration;

namespace FsTask.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication
                    .CreateBuilder(new WebApplicationOptions
                    {
                        Args = args,
                        ContentRootPath = Environment.CurrentDirectory
                    });

            builder.Host.ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json");
            });
            builder.Services.Configure<QuestDbConfig>(builder.Configuration.GetSection("QuestDbConfig"));


            builder.Services.AddSingleton<QuestDbConfig>(sp => new QuestDbConfig
            {
                Host = "localhost",
                Port = 9009,
                UserName = "admin",
                Password = "quest",
                DataBase = "qdb",
                NpsqlPort = 8812
            });

            builder.Services.AddControllersWithViews();

            FsTaskBootstrapper.Run(builder.Services);

            builder.Services.AddHostedService<PullingSensorDataFromQueueHostedService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
            }

            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}