using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Models;

using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace WinApp.Controllers
{
    class AccountController : BaseController
    {
        public ActionResult Default()
        {
            System.Diagnostics.Debug.WriteLine("Ctrl/Acc/Default");
           var db = Collection;
            var json = GetAsync("https://localhost:44395/Account/ApiIndexAsync").Result;
            System.Diagnostics.Debug.WriteLine(json);
            if (json.Equals(null))
            {
                System.Diagnostics.Debug.WriteLine("Ctrl/Acc/Default2");
                return Post(CreateApiContext(null, null, "/account/select"), o => {
                    foreach (var e in o.ToObject<List<Models.Account>>())
                    {
                        System.Diagnostics.Debug.WriteLine(e.Id);
                        db.Insert(e.Id, e);
                    }

                    RedirectToAction("Default");               
                });
            }
            System.Diagnostics.Debug.WriteLine("Ctrl/Acc/Default3");
            List< Models.Account> result = JsonConvert.DeserializeObject< List<Models.Account>>(json);
            return View(result);
        }
        public ActionResult All()
        {
            return View(Collection.ToList<Models.Account>());
        }
        public ActionResult Clear()
        {
            Collection.Clear();
            return GoFirst();
        }
        public ActionResult Create(Models.UserCreate userCreate)
        {
            return Post(CreateApiContext(userCreate, userCreate.Id, Env.SignupAPI ), o => {
                RedirectToAction("Default");
            });
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }
        public ActionResult Update(string action, string un, string role)
        {
            var db = Collection;
            var e = new Models.Account { Id = un, Role = Str2Permission(role) };
            return Post(CreateApiContext(e, un, "account/" + action), o => {
                switch (action[0])
                {
                    case 'i': db.Insert(un, o); break;
                    case 'u': db.Update(un, o); break;
                    case 'd': db.Delete(un); break;
                }
                RedirectToAction("Default");
            });
        }
        
        public static Permission Str2Permission(string role)
        {
            switch (role)
            {
                case "ADMIN":
                    return Permission.ADMIN;
                case "MOD":
                    return Permission.MOD;
                case "USER":
                    return Permission.USER;
                default:
                    return Permission.NONE;
            }
            return Permission.NONE;
        }

        public async Task<string> GetAsync(string uri)
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
