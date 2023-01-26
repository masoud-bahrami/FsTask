namespace FsTask.AcceptanceTests.Helpers;

public static class HttpResponseMessageExtensions
{
    public static void EnsureSuccess(this HttpResponseMessage message)
    {
        if (message.IsSuccessStatusCode)
            return;

        throw new Exception(message.Content.ReadAsStringAsync().Result);
    }

    public static T To<T>(this HttpResponseMessage httpResponse)
    {
        var result = httpResponse.Content.ReadAsStringAsync().Result;

        return result.Deserialize<T>();
    }
}