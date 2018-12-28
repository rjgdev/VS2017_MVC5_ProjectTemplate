using System.Web.Mvc;
using Application.Web.Models;
using log4net;

namespace Application.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILog Log = Logger.LoggingInstance;

        public void Attention(string message)
        {
            TempData.Add(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            TempData.Add(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Add(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Add(Alerts.ERROR, message);
        }
        
    }
}