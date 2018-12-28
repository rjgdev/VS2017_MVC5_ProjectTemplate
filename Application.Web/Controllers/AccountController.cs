using Application.Web.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using Application.Web.Models;
using RestSharp;
using Newtonsoft.Json;
using Application.Web.Models.ViewModels;
using System.Linq;

namespace Application.Web.Controllers
{

    public class AccountController : BaseController
    {

        //private readonly IOwinContext _iOwinContext;

        private static readonly HttpClient client = new HttpClient();

        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <param name="inputPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string inputEmail, string inputPassword)
          {

            var url = "token";
            var response = await HttpClientHelper.ApiTokenCall(url, "password", inputEmail, inputPassword);

            if (response.IsSuccessful)
            {
                var result = response.Content;
                var token = JsonConvert.DeserializeObject<Models.ViewModels.TokenViewModel>(result);
                AuthenticationProperties options = new AuthenticationProperties();

                options.AllowRefresh = true;
                options.IsPersistent = true;
                options.ExpiresUtc = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                System.Web.HttpContext.Current.GetOwinContext().Authentication.SignIn(options);

                //var customer = new CustomerViewModel();
                //var getDomainUrl = "api/account/getdomain/" + token.UserName + "/";
                //response = await HttpClientHelper.ApiCall(getDomainUrl, Method.GET);

                //if (response.IsSuccessful)
                //{
                //    result = response.Content;
                //    customer = JsonConvert.DeserializeObject<CustomerViewModel>(result);

                var claims = new[]
                {
                            //Email Address
                    new Claim(ClaimTypes.Name, token.LastName),
                    //Access Token
                    new Claim("AccessToken", token.AccessToken),
                    //Expiration Date
                    new Claim("ExpiryDate", token.Expires.ToString()),
                    new Claim("Domain", token.Domain),
                    new Claim("CustomerId", token.CustomerId.ToString()),
                    new Claim("FirstName", token.FirstName.ToString()),
                    new Claim("LastName", token.LastName.ToString()),
                    new Claim("MiddleName", token.MiddleName.ToString()),
                    //new Claim("UserName", token.UserName.ToString()),
                    new Claim("EmailAddress", token.UserName.ToString()),
                    new Claim("Image", token.Image.ToString()),
                    new Claim("Role", token.Role.ToString()),
                };

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
                //if(CookieHelper.IsInRole("Administrator"))
                //{
                //    return RedirectToAction("Index", "Brand");
                //}
                return RedirectToAction("Index", "Home");

                //}
                //Error("An error has occurred");
                //Log.Error(string.Format(Type.GetType(typeof(AccountController).Name) + "||Update||DeliveryRequest ID::{0}||API Response::{1}", token.UserName, response));
                //return RedirectToAction("Login","Account");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["LoginMessage"] = "Login failed. Email Address or password supplied does not exist.";
                return View("Login");
            }
            else return RedirectToAction("Http403", "Error");


            //HttpResponseMessage responseMessage = await client.SendAsync(request);
            //if (responseMessage.IsSuccessStatusCode)
            //{

            //    string resultContent = responseMessage.Content.ReadAsStringAsync().Result;
            //    var token = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ViewModels.TokenViewModel>(resultContent);

                

            //    //var customer = new CustomerViewModel();
            //    //var getDomainUrl = "api/Account/GetDomain/" + token.UserName;

            //    //var response = await HttpClientHelper.ApiCall(getDomainUrl, Method.GET);

            //    //if (response.IsSuccessful)
            //    //{
            //    //    var result = response.Content;
            //    //    customer = JsonConvert.DeserializeObject<CustomerViewModel>(result);
            //    //    var claims = new[]
            //    //    {
            //    //        //Email Address
            //    //        new Claim(ClaimTypes.Name, token.UserName),
            //    //        //Access Token
            //    //         new Claim("AccessToken", token.AccessToken),
            //    //        //Expiration Date
            //    //        new Claim("ExpiryDate", token.Expires.ToString()),
            //    //        new Claim("Domain", customer.Domain),
            //    //        new Claim("CustomerId", customer.Id.ToString()),
            //    //    };

            //    //    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            //    //    System.Web.HttpContext.Current.GetOwinContext().Authentication.SignIn(options, identity);
            //    //    return RedirectToAction("Index", "Home");
            //    //}
            //    //else
            //    //{
            //    //    return View("Login");
            //    //}


            //}
            //else if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
            //{
            //    TempData["Message"] = "Login failed. Email Address or password supplied doesn't exist.";
            //    return View("Login");
            //}
            //else return RedirectToAction("Http403","Error");

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
                if(verifyEmailResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, inputEmail);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                    else return RedirectToAction("Http403", "Error");
                }
                else if(verifyEmailResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["Message"] = "Email Address doesn't exist";
                    return View(inputEmail);
                }
                else return RedirectToAction("Http403", "Error");
            }
        }

        // POST: /Account/LogOff
        //[HttpPost]
        public ActionResult LogOff()
        {
       
            var identity = (ClaimsIdentity)System.Web.HttpContext.Current.User.Identity;
            

            var claimNameList = identity.Claims.Select(x => x.Type).ToList();
            
            foreach (var name in claimNameList)
            {
                var claim = identity.Claims.FirstOrDefault(x => x.Type == name);
                identity.RemoveClaim(claim);
                
            }

            //if ( claim.AuthenticationType != null)
            //{
            //    System.Web.HttpCookie myCookie = new System.Web.HttpCookie(DefaultAuthenticationTypes.ApplicationCookie);
            //    myCookie.Expires = DateTime.Now.AddDays(-1d);
            //    Response.Cookies.Add(myCookie);
            //}
            System.Web.HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TempData["LoginMessage"] = "";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<ActionResult> RegisterUser(string auth)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(auth);
            long customerId = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));

            RegisterBindingModel user = new RegisterBindingModel();
            user.CustomerId = customerId;

            return View(user);
        }

        //
        // POST: /Account/Register User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterUser(RegisterBindingModel user)
        {

            var url = "api/Account/Register";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, user);
            if (response.IsSuccessful)
            {
                TempData["LoginMessage"] = "User successfully created!";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Error("An error has occurred");
                return RedirectToAction("Login", "Account");
            }

            //var url = new Uri(ConfigHelper.BaseUrl) + "api/Account/Register/";

            //HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, user);

            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    TempData["LoginMessage"] = "User successfully created!";
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            //    Error("An error has occurred");
            //    return RedirectToAction("Login", "Account");
            //}


        }
        /// <summary>
        ///  GET: /Account/Register Customer
        /// </summary>
        /// <returns></returns>
        //
        public ActionResult RegisterCustomer()
        {

            return View();
        }

        /// <summary>
        /// POST: /Account/Register Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> RegisterCustomer(Customer customer)
        //{
        //    var url = new Uri(ConfigHelper.BaseUrl) + "api/customer/add";
        //    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, customer);

        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        TempData["Message"] = "Account Successfully Created";
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
        //    {
        //        return View(customer);
        //    }
        //    else return View(customer);

        //}

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string inputEmail,string newPassword, string confirmPassword)
        {
            if(newPassword != confirmPassword)
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

        [HttpPost]
        public async Task<ActionResult> CustomerRegister(CustomerViewModel customer)
        {


            var url = "api/customer/CustomerSignUp";
            var response = await HttpClientHelper.ApiCall(url, Method.POST, customer);
            return RedirectToAction("Login", "Account");
        }


    }

    public class RegisterBindingModel
    {

        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public long CustomerId { get; set; }

    }


}
