using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Application.Web.Helper
{

    public static class CookieHelper
    {

        public static string Token
        {
            get { return ClaimValue("AccessToken"); }
        }

        public static string ExpiryDate
        {
            get { return ClaimValue("ExpiryDate"); }
        }

        public static string Domain
        {
            get { return ClaimValue("Domain"); }
        }

        public static string CustomerId
        {
            get { return ClaimValue("CustomerId"); }
        }

        public static string FirstName
        {
            get { return ClaimValue("FirstName"); }
        }

        public static string LastName
        {
            get { return ClaimValue("LastName"); }
        }
        public static string MiddleName
        {
            get { return ClaimValue("MiddleName"); }
        }

        public static string EmailAddress
        {
            get { return ClaimValue("EmailAddress"); }
        }

        //public static string UserName
        //{
        //    get { return ClaimValue("UserName"); }
        //}

        public static string Image
        {
            get { return ClaimValue("Image"); }
        }

        public static string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public static string Role
        {
            get { return ClaimValue("Role"); }
        }

        public static string ClaimValue(string key)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var token = identity.Claims.FirstOrDefault(c => c.Type == key)?.Value;

            return token;
        }
    }

    public class MyUser
    {
        public static bool IsInRole(string role)
        {
            if (CookieHelper.Role.ToLower() == role.ToLower())
            {
                return true;
            }
            else return false;
        }
    }
    

}