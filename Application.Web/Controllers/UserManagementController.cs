using Application.Web.Helper;
using Application.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Application.Web.Controllers
{
    public class UserManagementController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: UserManagement
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserManagement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserManagement/Create
        [HttpPost]
        public ActionResult Create(ApplicationUser user)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserManagement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ApplicationUser user)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserManagement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ApplicationUser user)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ForgotPassword()
        {
            return View("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string inputEmail)
        {
            var url = new Uri(ConfigHelper.BaseUrl) + "ForgotPassword";
            var verifyEmailUrl = new Uri(ConfigHelper.BaseUrl) + "VerifyEmail";

            using (HttpResponseMessage verifyEmailResponse = await client.PostAsJsonAsync(verifyEmailUrl, inputEmail))
            {
                if (verifyEmailResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, inputEmail);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                    else return RedirectToAction("Http403", "Error");
                }
                else if (verifyEmailResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["Message"] = "Email Address doesn't exist";
                    return View(inputEmail);
                }
                else return RedirectToAction("Http403", "Error");
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string inputEmail, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                TempData["NotMatch"] = "Password ";
            }
            var url = new Uri(ConfigHelper.BaseUrl) + "api/user/changePassword";
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, inputEmail);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Message"] = "Password successfully changed";
                return RedirectToAction("Login", "Account");
            }
            else if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Error. Password successfully changed";
                return View(inputEmail);
            }
            else return View(inputEmail);
        }
    }
}
