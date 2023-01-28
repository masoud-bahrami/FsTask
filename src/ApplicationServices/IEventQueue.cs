using FsTask.Domain.Contract;
using System.Collections.Concurrent;

namespace FsTask.ApplicationServices;

public interface IEventQueue
{
    void Queue(string at, StoreSensorDataCommand command);
    public (string, StoreSensorDataCommand) Dequeue();
}


public class EventQueue : IEventQueue
{
    private static readonly ConcurrentQueue<(string, StoreSensorDataCommand)> _queue = new();
    public virtual void Queue(string at, StoreSensorDataCommand command)
    {
        _queue.Enqueue((at, command));
    }

    public (string, StoreSensorDataCommand) Dequeue()
    {
        _queue.TryDequeue(out var result);
        return result;
    }
}

public class MyEventQueue : EventQueue
{
    private readonly ISensorEventsService service;

    public MyEventQueue(ISensorEventsService service)
    {
        this.service = service;
    }

    public override void Queue(string at, StoreSensorDataCommand command)
    {
        base.Queue(at, command);

        var dequeue = base.Dequeue();
        service.Save(dequeue.Item2, at: dequeue.Item1).Wait();
    }
}