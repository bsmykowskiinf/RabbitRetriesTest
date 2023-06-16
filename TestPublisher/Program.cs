using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

static class Program
{
  static async Task Main()
  {
    var numberOfTestMessages = 100;

    Console.Title = "Samples.ErrorHandling.WithDelayedRetries";
    var defaultFactory = LogManager.Use<DefaultFactory>();
    defaultFactory.Level(LogLevel.Warn);
    defaultFactory.Directory("..\\..\\..\\resultLogs");

    var endpointConfiguration = new EndpointConfiguration(CommonConfiguration.publisherEndpointName);
    endpointConfiguration.UsePersistence<LearningPersistence>();
    endpointConfiguration.LicensePath(CommonConfiguration.licensePath);

    ConfigureTransport(CommonConfiguration.rabbitConnectionString, endpointConfiguration);

    var endpointInstance = await Endpoint.Start(endpointConfiguration)
        .ConfigureAwait(false);
    Console.WriteLine($"Sending {numberOfTestMessages} test messages.");
    Console.WriteLine("Press any key to exit");

    for (int i = 0; i < numberOfTestMessages; i++)
    {
      var myMessage = new MyMessage
      {
        Id = Guid.NewGuid()
      };
      await endpointInstance.Send(myMessage)
          .ConfigureAwait(false);
    }

    var key = Console.ReadKey();

    Console.WriteLine("Stopping");
    await endpointInstance.Stop()
        .ConfigureAwait(false);
  }

  private static void ConfigureTransport(string rabbitConnectionString, EndpointConfiguration endpointConfiguration)
  {
    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

    transport.ConnectionString(rabbitConnectionString);
    transport.UseConventionalRoutingTopology(QueueType.Classic);
    transport.Transactions(TransportTransactionMode.ReceiveOnly);

    transport.PrefetchMultiplier(10);
    ConfigRouting(transport);
  }

  private static void ConfigRouting(TransportExtensions transport)
  {
    transport.Routing().RouteToEndpoint(
      typeof(MyMessage).Assembly,
      CommonConfiguration.handlerEndpointName);
  }

}