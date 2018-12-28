using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Application.Web.Helper
{
    public class HttpGetHelper
    {
        public static async Task<HttpResponseMessage> ApiCall(string mediaTypeWithQualityHeaderValue,string authenticationScheme, string authenticationParameter, string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigHelper.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeWithQualityHeaderValue));
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationScheme, authenticationParameter);
                HttpResponseMessage resMessage = await client.GetAsync(url);
                return resMessage;
            }
           
        }

    }
}