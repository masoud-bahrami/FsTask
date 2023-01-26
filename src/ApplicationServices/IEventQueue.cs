using System.Collections.Concurrent;
using FsTask.Domain.Contracts;

namespace FsTask.ApplicationServices;

public interface IEventQueue
{
    void Queue(string at, StoreSensorDataCommand command);
    public (string, StoreSensorDataCommand) Dequeue();
}

public class EventQueue : IEventQueue
{
    private static readonly ConcurrentQueue<(string , StoreSensorDataCommand)> _queue = new ();
    public void Queue(string at, StoreSensorDataCommand command)
    { 
        _queue.Enqueue((at , command));
    }

    public (string, StoreSensorDataCommand) Dequeue()
    {
        _queue.TryDequeue(out var result);
        return result;
    }
}