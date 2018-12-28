using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Application.Api.Filters;
using Application.Api.Models;
using Newtonsoft.Json.Linq;

namespace Application.Api.Controllers
{
    /// <summary>
    /// File uploading API
    /// </summary>
    [Authorize]
    [RequireHttps]
    [RoutePrefix("api/DocumentUpload")]
    public class FileUploadController : BaseApiController
    {
        /// <summary>
        ///     Upload Document.....
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("MediaUpload")]
        public async Task<HttpResponseMessage> MediaUpload()
        {
            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            var formData = provider.FormData;
            //access files  
            IList<HttpContent> files = provider.Files;

            var file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

            ////-------------------------------------For testing----------------------------------  
            //to append any text in filename.  
            //var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"') + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //ToDo: Uncomment this after UAT as per Jeeevan  

            //List<string> tempFileName = thisFileName.Split('.').ToList();  
            //int counter = 0;  
            //foreach (var f in tempFileName)  
            //{  
            //    if (counter == 0)  
            //        thisFileName = f;  

            //    if (counter > 0)  
            //    {  
            //        thisFileName = thisFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + f;  
            //    }  
            //    counter++;  
            //}  

            ////-------------------------------------For testing----------------------------------  
            try
            {
                var filename = string.Empty;
                var input = await file1.ReadAsStreamAsync();
                var directoryName = string.Empty;
                var URL = string.Empty;
                var tempDocUrl = ConfigurationManager.AppSettings["DocsUrl"];

                if (!string.IsNullOrEmpty(formData["DocType"]))
                {
                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = Path.Combine(path, @"ClientDocument\\Image");
                    filename = Path.Combine(directoryName, thisFileName);

                    //Deletion exists file  
                    if (File.Exists(filename))
                        File.Delete(filename);

                    var DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                    URL = DocsPath + thisFileName;
                }
                else 
                {
                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = Path.Combine(path, @"ClientDocument\\Document");
                    filename = Path.Combine(directoryName, thisFileName);

                    //Deletion exists file  
                    if (File.Exists(filename))
                        File.Delete(filename);

                    var DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                    URL = DocsPath + thisFileName;
                }


                //Directory.CreateDirectory(@directoryName);  
                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("DocsUrl", URL);

                Log.Info(typeof(FileUploadController).FullName + $"||{UserEnvironment}||MediaUpload||FileName::{thisFileName}||Successfully uploaded.");

                return response;
            }
            catch (Exception e)
            {
                Log.Error(typeof(FileUploadController).FullName, e);
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Upload base64 string with filename
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Base64MediaUpload")]
        public HttpResponseMessage Base64MediaUpload(object obj)
        {
            try
            {
                var filename = new Common().ConvertSaveImage(obj);
                var response = Request.CreateResponse(HttpStatusCode.OK);

                Log.Info(typeof(FileUploadController).FullName + $"||{UserEnvironment}||Base64MediaUpload||FileName::{filename}||Successfully uploaded.");

                return response;
            }
            catch (Exception e)
            {
                Log.Error(typeof(FileUploadController).FullName, e);
                Console.WriteLine(e);
                throw;
            }
        }
    }
}