using Newtonsoft.Json;
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
        public static string BaseURL = "https://localhost:44396/";
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

        public class Menu
            {
            public string Name;
            public string URL;
            public List<Menu> Menuitems;
            }   


        public async Task<string> PostAsync(string URL, object contents)
        {

            var request = (HttpWebRequest)System.Net.WebRequest.Create(URL);


            var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(contents));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
    }
}
