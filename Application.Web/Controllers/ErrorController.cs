using System;
using System.Web.Mvc;
using log4net;

namespace Application.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILog _log = Logger.LoggingInstance;

        public ActionResult General(Exception exception)
        {
            ViewBag.ErrorCode = Response.StatusCode;
            ViewBag.Message = exception.Message;

            //you should log your exception here
            _log.Error(exception.Message, exception);
            return View();
        }

        public ActionResult Http401()
        {
            ViewBag.ErrorCode = Response.StatusCode;
            ViewBag.Message = "Unauthorized.";

            _log.Error(string.Format("HTTP StatusCode::{0}||Message::{1}", ViewBag.ErrorCode, ViewBag.Message));
            return View();
        }

        public ActionResult Http403()
        {
            ViewBag.Message = Response.StatusCode;
            ViewBag.Message = "Forbidden.";

            _log.Error(string.Format("HTTP StatusCode::{0}||Message::{1}", ViewBag.ErrorCode, ViewBag.Message));
            return View();
        }

        public ActionResult Http404()
        {
            ViewBag.ErrorCode = Response.StatusCode;
            ViewBag.Message = "Not Found.";

            _log.Error(string.Format("HTTP StatusCode::{0}||Message::{1}", ViewBag.ErrorCode, ViewBag.Message));
            return View();
        }

        public ActionResult Lockout()
        {
            ViewBag.Message = Response.StatusCode;
            ViewBag.Message = "Account locked out.";

            _log.Error(string.Format("HTTP StatusCode::{0}||Message::{1}", ViewBag.ErrorCode, ViewBag.Message));
            return View();
        }
    }
}