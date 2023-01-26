using System.Text;

namespace FsTask.AcceptanceTests.Helpers;

public static class StringContentExtensions
{
    public static StringContent ToStringContent<T>(this T con, string mediaType = "application/json")
        => con.ToStringContent(Encoding.UTF8, mediaType);

    public static StringContent ToStringContent<T>(this T con, Encoding encoding, string mediaType = "application/json")
        => new(con.Serialize(), encoding, mediaType);
}