using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace WepApp.Controllers
{
    public class DeviceController : BaseController
    {
        // GET: Device
        public ActionResult Index()
        {
            
            return View(Collection.ToList<Models.Device>());
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