using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;
using DEV = Models.DeviceViewModel;

namespace DeviceDemo.Controllers
{
    

    class DeviceController : BaseController
    {

        static DEV _selected;
        static List<DEV> _devices;
        static void Subcribe()
        {
            //if (!MqttController.IsConnected)
             //   MqttController.Connect();
            if (MqttController.IsConnected)
            {

                MqttController.Broker.Subscribe(
                    new string[] { "statusreply/" + _selected.Id }, 
                    new byte[] { 0 });

                System.Diagnostics.Debug.WriteLine(_selected.Id);
            }
        }
        static void Publish(int value)
        {
            if (MqttController.IsConnected)
            {

                MqttController.Broker.Publish(
                    "status/" + _selected.Id, 
                    Encoding.ASCII.GetBytes("{\"value\":" + value + "}"));
                System.Diagnostics.Debug.WriteLine(_selected.Id);
                System.Diagnostics.Debug.WriteLine(value);
            }
        }
        public ActionResult Demo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Redirect("home");
            }

            var s = new Models.DeviceStatus();
            for (int i = 0; i < 4; i++)
                s.Add("LED" + i, 0);

            //Subcribe();

            
            _selected = new DEV { Id = id, Status = s };
            Subcribe();
            Engine.CreateThread(MqttController.Connect);
            _selected.Changed += (d, v) => {
                Publish(v);
                Engine.CreateThread(MqttController.Connect);
            };
            
            

            return View(_selected);
        }
        public ActionResult StatusReply(string id, JObject o)
        {
            System.Diagnostics.Debug.WriteLine("StatusReply");
            _selected.UpdateStatus(o.Get<int>("value"));


            return View(_selected);
        }
        // void SetValue(int value)
        //{
        //  int i = 0;
        //    foreach (var p in _leds)
        //{
        //  p.Value.State = value & (1 << i++);
        //}
        //  Model.UpdateStatus(value);
        // }
    }
}
