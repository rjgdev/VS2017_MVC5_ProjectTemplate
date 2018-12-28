using log4net;
using log4net.Config;

namespace Application.Api
{
    public class Logger
    {
        static Logger()
        {
            XmlConfigurator.Configure();
        }

        public static ILog LoggingInstance { get; } = LogManager.GetLogger("CenGts API");
    }
}