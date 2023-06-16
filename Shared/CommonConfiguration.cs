namespace Shared
{
  public class CommonConfiguration
  {
    public static readonly string handlerEndpointName = "testRetries_Handler";
    public static readonly string publisherEndpointName = "testRetries_Publisher";
    public static readonly string licensePath = "{licensePath}";
    public static readonly string rabbitConnectionString = "host=localhost;virtualhost=test;username={username};password={password};usetls=false";
  }
}
