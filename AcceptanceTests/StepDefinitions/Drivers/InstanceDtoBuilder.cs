using FsTask.Domain.Contract;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

public class InstanceDtoBuilder
{
    private string _humanId = "1";
    private double _positionX = 5.6;
    public double _PositionY = 5.6;
    private int _velX = 0;
    private int _velY = 0;

    public static InstanceDtoBuilder INeedANewInsatnce()
    {
        return new InstanceDtoBuilder();
    }

    public InstanceDtoBuilder WithHumanId(string humanId)
    {
        _humanId = humanId;
        return this;
    }

    public InstanceDtoBuilder WithPositionX(double positionX)
    {
        _positionX = positionX;
        return this;
    }

    public InstanceDtoBuilder WithPositionY(double positionY)
    {
        _PositionY = positionY;
        return this;
    }

    public InstanceDtoBuilder WithVelX(int velX)
    {
        _velX = velX;
        return this;
    }

    public InstanceDtoBuilder WithVelY(int velY)
    {
        _velY = velY;
        return this;
    }
    public InstanceDto ThankYou()
    {
        return new InstanceDto
        {
            HumanId = _humanId,
            PositionX = _positionX,
            PositionY = _PositionY,
            VelX = _velX,
            VelY = _velY
        };
    }

}