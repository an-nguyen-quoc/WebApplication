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
            var db = Collection;
            var lst = db.ToList<Models.Device>();
            if (lst.Count == 0)
            {
                var signals = CreateSignals();
                //for (int i = 0; i < 4; i++)
                //{
                //    var key = "LED" + i;
                //    signals.Add(key, 0);
                //}
                for (int i = 0; i < 10; i++)
                {
                    string id = string.Format("LTNC{0:0000}", i + 1);
                    var device = new Models.Device { Name = id, Status = signals };

                    db.Insert(id, device);
                    device.Id = id;
                    lst.Add(device);
                }
            }
            return View(lst);
            //return View(Collection.ToList<Models.Device>());
        }

        [HttpGet]
        [Route("ApiIndexAsync")]
        public async System.Threading.Tasks.Task<string> ApiIndexAsync()
        {
           
            System.Diagnostics.Debug.WriteLine("Device/ApiIndex");
            var danh_sach = Collection.ToList<Models.Device>();
            var json = JsonConvert.SerializeObject(danh_sach, Formatting.Indented);
            return await Task.FromResult(json);
        }

        [HttpGet]
        [Route("GetDeviceAsync")]
        public async System.Threading.Tasks.Task<string> GetDeviceAsync(string id)
        {

            System.Diagnostics.Debug.WriteLine("Device/GetDevice");
            var danh_sach = Collection.FindById<Models.Device>(id);
            var json = JsonConvert.SerializeObject(danh_sach, Formatting.Indented);
            return await Task.FromResult(json);
        }

        [HttpPost]
        [Route("UpdateStatus")]
        public async System.Threading.Tasks.Task<string> UpdateStatusAsync(string id, string status)
        {

            System.Diagnostics.Debug.WriteLine("Device/UpdateStatus");
            var danh_sach = Collection.FindById<Models.Device>(id);
            var json = "";
            if (danh_sach == null)
                json = "Khong ton tai thiet bi";
            //var status = GetApiObject<DeviceStatus>();
            

            try
            {
                //var _deviceView = new DeviceViewModel();
                //_deviceView.UpdateStatus(status);
                //danh_sach.Status = _deviceView.Status;
                danh_sach.Status = (DeviceStatus)status;
                Collection.Update(danh_sach.Id, danh_sach);
                json = "Thay doi thanh cong";
            }
            catch(Exception e)
            {
                json = "Khong the thay doi trang thai";
            }

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