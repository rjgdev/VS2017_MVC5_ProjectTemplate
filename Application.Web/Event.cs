using log4net;
using log4net.Config;

namespace Application.Web
{
    public class Logger
    {
        static Logger()
        {
            XmlConfigurator.Configure();
        }

        public static ILog LoggingInstance { get; } = LogManager.GetLogger("Application Web");
    }
}