using FsTask.ApplicationServices;

namespace FsTask.API;

public class PullingSensorDataFromQueueHostedService : IHostedService, IDisposable
{
    private IServiceProvider Services { get; }
    private Timer? _timer = null;

    public PullingSensorDataFromQueueHostedService( IServiceProvider services)
    {
        Services = services;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        using var scope = Services.CreateScope();
        var _eventQueue =
            scope.ServiceProvider
                .GetRequiredService<IEventQueue>();

        var dequeue = _eventQueue.Dequeue();

        if (!string.IsNullOrWhiteSpace(dequeue.Item1))
        {
            var service = scope.ServiceProvider.GetRequiredService<ISensorEventsService>();
            service.Save(dequeue.Item2, at: dequeue.Item1).Wait();
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}