using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

static class Program
{
  static readonly int immediateRetries = 2;
  static readonly int delayedRetries = 3;
  static readonly int delayedRetriesTimeIncrease = 10;
  static readonly int preferchCountMultiplier = 10;
  static readonly int processingConcurrencyLimit = 3;

  static async Task Main()
  {

    Console.Title = "Samples.ErrorHandling.WithDelayedRetries";
    var defaultFactory = LogManager.Use<DefaultFactory>();
    defaultFactory.Level(LogLevel.Info);
    defaultFactory.Directory("..\\..\\..\\resultLogs");

    var endpointConfiguration = new EndpointConfiguration(CommonConfiguration.handlerEndpointName);
    endpointConfiguration.UsePersistence<LearningPersistence>();
    endpointConfiguration.LicensePath(CommonConfiguration.licensePath);

    endpointConfiguration.LimitMessageProcessingConcurrencyTo(processingConcurrencyLimit);

    ConfigureTransport(endpointConfiguration);
    ConfigureRecoverability(endpointConfiguration);

    var endpointInstance = await Endpoint.Start(endpointConfiguration)
        .ConfigureAwait(false);


    while (true)
    {
      var key = Console.ReadKey();
      if (key.Key != ConsoleKey.Enter)
      {
        break;
      }
    }
    await endpointInstance.Stop()
        .ConfigureAwait(false);
  }

  private static void ConfigureRecoverability(EndpointConfiguration endpointConfiguration)
  {
    var recoverability = endpointConfiguration.Recoverability();
    recoverability.Immediate(
        customizations: immedate =>
        {
          immedate.NumberOfRetries(immediateRetries);
        });
    recoverability.Delayed(
        customizations: delayed =>
        {
          delayed.NumberOfRetries(delayedRetries).TimeIncrease(TimeSpan.FromSeconds(delayedRetriesTimeIncrease));
        });
  }

  private static void ConfigureTransport(
    EndpointConfiguration endpointConfiguration)
  {
    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

    transport.ConnectionString(CommonConfiguration.rabbitConnectionString);
    transport.UseConventionalRoutingTopology(QueueType.Classic);
    transport.Transactions(TransportTransactionMode.ReceiveOnly);

    transport.PrefetchMultiplier(preferchCountMultiplier);
  }

}