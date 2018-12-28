using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Application.Web.Helper
{
    public static class ConfigHelper
    {
        public static string BaseUrl
        {
            get { return (ConfigurationManager.AppSettings["BaseUrl"]);  }
        }


        //public static string SendAddress
        //{
        //    get { return (ConfigurationManager.AppSettings["SendAddress"]); }
        //}

        public static long ForReceivingStatus
        {
            get { return Convert.ToInt64(ConfigurationManager.AppSettings["ForReceivingStatus"]);  }
        }

        public static long CompletedStatus
        {
            get { return Convert.ToInt64(ConfigurationManager.AppSettings["CompletedStatus"]); }
        }

        public static long ReceivedStatus
        {
            get { return Convert.ToInt64(ConfigurationManager.AppSettings["ReceivedStatus"]); }
        }
    }
}