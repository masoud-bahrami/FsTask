using FsTask.Domain.Contract;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

public class StoreSensorDataCommandBuilder
{
    private List<InstanceDto> _instances;

    public StoreSensorDataCommandBuilder()
    {
        _instances = new List<InstanceDto>();
    }

    public static StoreSensorDataCommandBuilder INeed()
    {
        return new StoreSensorDataCommandBuilder();
    }

    public StoreSensorDataCommandBuilder WithInstance(InstanceDto instance)
    {
        _instances.Add(instance);
        return this;
    }
    public StoreSensorDataCommand ThankYou()
    {
        return new StoreSensorDataCommand
        {
            Instances = _instances
        };
    }
}