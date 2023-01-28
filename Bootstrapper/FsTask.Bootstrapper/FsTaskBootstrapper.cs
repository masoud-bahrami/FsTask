using FsTask.ApplicationServices;
using FsTask.QuestDB;
using Microsoft.Extensions.DependencyInjection;

namespace FsTask.Bootstrapper
{
    public static class FsTaskBootstrapper
    {
        public static void Run(IServiceCollection builderServices)
        {
            builderServices.AddScoped<IEventQueue, EventQueue>();
            builderServices.AddScoped<ISensorEventsService, SensorEventsToGuestDbService>();
        }

        public static void RunTest(IServiceCollection builderServices)
        {
            builderServices.AddScoped<IEventQueue, MyEventQueue>();
            builderServices.AddSingleton<ISensorEventsService, SensorEventsService>();
            //builderServices.AddScoped<ISensorEventsService, SensorEventsToGuestDbService>();
        }
    }
}