namespace FsTask.Domain;

public class Position
{
    public double Y { get; init; }
    public double X { get; init; }
    
    public static Position New(double x, double y) => new Position(x, y);
    public Position ChangeX(double x) => new Position(x, Y);
    public Position ChangeY(double y) => new Position(X, y);


    public Position(double x, double y)
    {
        CalibbrateXandY(x, y);

        X = x;
        Y = y;
    }
    private static void CalibbrateXandY(double x, double y)
    {
        //TODO normalize x and y position
    }

    

    public override bool Equals(object? obj)
    {
        var that = (Position)obj;
        return this.X == that.X
               && this.Y == that.Y;
    }
}