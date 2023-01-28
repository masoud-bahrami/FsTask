namespace FsTask.QuestDB;

public class QuestDbConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName = "admin";
    public string Password = "quest";
    public string DataBase = "qdb";
    public int NpsqlPort = 8812;
}