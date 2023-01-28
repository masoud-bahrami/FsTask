namespace FsTask.Domain.Exception;

public class HumanBeingInDifferentPositionAtTheSameTimeException : System.Exception
{
    public HumanBeingInDifferentPositionAtTheSameTimeException(string dtoInstanceHumanId)
    : base(dtoInstanceHumanId)
    {
        

    }
}