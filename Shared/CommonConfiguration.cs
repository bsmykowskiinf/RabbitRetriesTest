using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
  public class CommonConfiguration
  {
    public static readonly string handlerEndpointName = "testRetries_Handler";
    public static readonly string publisherEndpointName = "testRetries_Publisher";
    public static readonly string licensePath = "D:\\Apps\\Tim\\NServiceBusLicense\\License.xml";
    public static readonly string rabbitConnectionString = "host=localhost;virtualhost=test;username=tim_user;password=guest;usetls=false";
  }
}
