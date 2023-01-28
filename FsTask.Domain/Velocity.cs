    namespace FsTask.Domain;

public class Velocity
{
    public double Y { get; init; }
    public double X { get; init; }
    public static Velocity New(double x, double y)
    {
        return new Velocity
        {
            X = x,
            Y = y
        };
    }

    public override bool Equals(object? obj)
    {
        var that = (Velocity)obj;
        return this.X == that.X
               && this.Y == that.Y;
    }
}