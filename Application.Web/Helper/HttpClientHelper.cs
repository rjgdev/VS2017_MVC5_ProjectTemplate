using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Application.Web.Helper
{
    public class HttpClientHelper
    {

        public static async Task<IRestResponse> ApiCall(string url, Method method)
        { 
  
            var client = new RestClient(ConfigHelper.BaseUrl + url);
            var request = new RestRequest(method);

            var certFile = HttpContext.Current.Server.MapPath("~/Certificates/wmslitecert.cer");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 0;
            ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

            X509Certificate2 certificates = new X509Certificate2();
            certificates.Import(certFile);
            client.ClientCertificates = new X509CertificateCollection() { certificates };
            client.Proxy = new WebProxy();
            client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + CookieHelper.Token);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response;

        }

        public static async Task<IRestResponse> ApiTokenCall(string url, string grantType, string username, string password)
        {

            var client = new RestClient(ConfigHelper.BaseUrl + url);
            var request = new RestRequest(Method.POST);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "grant_type="+grantType+"&username="+username+"&password="+password, ParameterType.RequestBody);

            var certFile = HttpContext.Current.Server.MapPath("~/Certificates/wmslitecert.cer");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
            X509Certificate2 certificates = new X509Certificate2();
            certificates.Import(certFile);
            client.ClientCertificates = new X509CertificateCollection() { certificates };
            client.Proxy = new WebProxy();

            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response;
        }

        public static async Task<IRestResponse> ApiCall(string url, Method method, Object obj)
        {
            var client = new RestClient(ConfigHelper.BaseUrl + url);
            var request = new RestRequest(method);
            var certFile = HttpContext.Current.Server.MapPath("~/Certificates/wmslitecert.cer");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

            X509Certificate2 certificates = new X509Certificate2();
            certificates.Import(certFile);
            client.ClientCertificates = new X509CertificateCollection(){certificates};
            client.Proxy = new WebProxy();
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + CookieHelper.Token);
            request.AddJsonBody(obj);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response;
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate,
                                                                X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        public class NewtonsoftJsonSerializer : ISerializer, IDeserializer
        {
            private Newtonsoft.Json.JsonSerializer serializer;

            public NewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
            {
                this.serializer = serializer;
            }

            public string ContentType
            {
                get { return "application/json"; } // Probably used for Serialization?
                set { }
            }

            public string DateFormat { get; set; }

            public string Namespace { get; set; }

            public string RootElement { get; set; }

            public string Serialize(object obj)
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                    {
                        serializer.Serialize(jsonTextWriter, obj);

                        return stringWriter.ToString();
                    }
                }
            }

            public T Deserialize<T>(RestSharp.IRestResponse response)
            {
                var content = response.Content;

                using (var stringReader = new StringReader(content))
                {
                    using (var jsonTextReader = new JsonTextReader(stringReader))
                    {
                        return serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }

            public static NewtonsoftJsonSerializer Default
            {
                get
                {
                    return new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });
                }
            }
        }

    }
}