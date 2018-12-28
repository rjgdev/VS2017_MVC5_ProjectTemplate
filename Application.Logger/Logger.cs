using log4net;
using log4net.Config;

namespace Application.Logger
{
    public static partial class Logger
    {
        static Logger()
        {
            XmlConfigurator.Configure();
        }

        public  static ILog LoggingInstance { get; } = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}