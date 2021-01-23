using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Controllers
{
    public class Env
    {
        public static string BaseURL = "https://localhost:44395/";
        public static string AccountAPI = BaseURL + "account/";
        public static string SignupAPI = BaseURL + "home/apisignup";
        public static string getDeviceAPI = BaseURL + "Device/ApiIndexAsync";

        public static async Task<string> GetAsync(string uri)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
