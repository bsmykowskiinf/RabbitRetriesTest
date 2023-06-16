using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
  ILog log;

  public MyMessageHandler() : base()
  {
    this.log = LogManager.GetLogger<MyMessageHandler>();
  }
  public Task Handle(MyMessage message, IMessageHandlerContext context)
  {
    log.Info($"Handling {nameof(MyMessage)} with MessageId:{context.MessageId}");
    throw new Exception("An exception occurred in the handler.");
  }
}
#endregion
