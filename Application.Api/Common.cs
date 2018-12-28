using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Application.Api
{
    /// <summary>
    /// Common functions
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Convert and saves the image from base64 string.
        /// </summary>
        public string ConvertSaveImage(object obj)
        {
            if (obj == null) return string.Empty;

            var objModel = JObject.Parse(obj.ToString());
            var uploadModel = objModel.ToObject<Base64ObjectModel>();

            if (string.IsNullOrEmpty(uploadModel.ImageString) && string.IsNullOrEmpty(uploadModel.FileName)) return string.Empty;

            byte[] imageBytes = Convert.FromBase64String(uploadModel.ImageString);

            var path = HttpRuntime.AppDomainAppPath;
            var directoryName = Path.Combine(path, @"ClientDocument\\Image");
            var filename = Path.Combine(directoryName, uploadModel.FileName);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Bitmap thumb = new Bitmap(100, 100);
                using (Image bmp = Image.FromStream(ms))
                {
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawImage(bmp, 0, 0, 100, 100);
                    }
                    bmp.Save(filename, ImageFormat.Jpeg);
                }
                
            }
            //File.WriteAllBytes(filename, imageBytes);

            return uploadModel.FileName;
        }
    }

    internal class Base64ObjectModel
    {
        public string ImageString { get; set; }
        public string FileName { get; set; }
    }
}