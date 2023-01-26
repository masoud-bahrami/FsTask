using FsTask.Domain.Contracts;

namespace FsTask.ApplicationServices;

class StoreSensorEventToGuestDbService : IStoreSensorEventService
{
    public Task Process(StoreSensorDataCommand cmd, string at)
    {
        throw new NotImplementedException();
    }

    public Task<SensorApiDataViewModel> Get(string from , string to )
    {
        throw new NotImplementedException();
    }

    public Task<SensorDataViewModel> Get(string from)
    {
        throw new NotImplementedException();
    }
}