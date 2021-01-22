using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models;
using Newtonsoft.Json;
//using System.Web.Script.Serialization;

namespace WepApp.Controllers
{
    public class DeviceController : BaseController
    {
        // GET: Device
        public ActionResult Index()
        {
            
            return View(Collection.ToList<Models.Device>());
        }

        [HttpGet]
        [Route("ApiIndex")]
        public async System.Threading.Tasks.Task<string> ApiIndexAsync()
        {
           
            System.Diagnostics.Debug.WriteLine("Device/ApiIndex");
            var danh_sach = Collection.ToList<Models.Device>();
            var json = JsonConvert.SerializeObject(danh_sach, Formatting.Indented);
            return await Task.FromResult(json);
        }
        Models.DeviceStatus CreateSignals()
        {
            var signals = new Models.DeviceStatus();
            for (int i = 0; i < 4; i++)
            {
                var key = "LED" + i;
                signals.Add(key, 0);
            }
            return signals;
        }

        public override object ApiInsert()
        {
            var device = GetApiObject<Models.Device>();
            device.Status = CreateSignals();

            Collection.Insert(device.Id, device);
            return Success(device);
        }
    }
}