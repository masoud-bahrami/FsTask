namespace FsTask.ApplicationServices;

public class Links
{
    public bool _hasNext;
    public bool _hasPreview;
    public Link _self { get; set; }
    public Link _preview { get; set; }
    public Link _next { get; set; }
    public int _total { get; set; }
}