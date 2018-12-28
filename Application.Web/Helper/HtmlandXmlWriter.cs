using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CenGts.Web.Helper
{
    public class HtmlAndXmlWriter
    {
        public async Task<dynamic> Escape(string badString)
        {
            return badString.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<");

        }

        public string GetHtmlFromOutObject(Object obj)
        {
            return "<div class='swal-text" + obj + "</div>";

        }
    }
}