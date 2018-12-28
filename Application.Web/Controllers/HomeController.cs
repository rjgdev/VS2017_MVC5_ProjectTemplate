using System.Web.Mvc;
using Application.Web.Helper;

namespace Application.Web.Controllers
{
    public class HomeController : Controller
    {
    
        [Authorize]
        public ActionResult Index()
        {
            var token = CookieHelper.Token;
            if (token != null) return View();
            else return RedirectToAction("Lockout", "Error");


        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
    }
}
