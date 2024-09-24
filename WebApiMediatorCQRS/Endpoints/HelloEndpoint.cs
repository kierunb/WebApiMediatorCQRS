using Reprise;

namespace WebApiMediatorCQRS.Endpoints;

[Endpoint]
public class HelloEndpoint
{
    [Get("repr/hello")]
    public static string Handle() => "Hello, world!";
}
