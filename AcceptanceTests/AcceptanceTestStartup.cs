using FsTask.Bootstrapper;
using FsTask.QuestDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FsTask.AcceptanceTests;

public class AcceptanceTestStartup
{
    private IConfiguration Configuration { get; }

    public AcceptanceTestStartup(IConfiguration configuration)
        => this.Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddControllers();

        FsTaskBootstrapper.RunTest(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseRouting();

        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}