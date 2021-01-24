using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;
using DEV = Models.DeviceViewModel;
using Newtonsoft;
using Newtonsoft.Json;

namespace WinApp.Controllers
{
    

    class DeviceController : BaseController
    {
        static DEV _selected;
        static List<DEV> _devices;
        public static List<DEV> Devices => _devices;

        public ActionResult Default()
        {


            if (_devices == null)
            {
                System.Diagnostics.Debug.WriteLine("Device null");
                string json = Env.GetAsync(Env.getDeviceAPI).Result;
                //var _object = 
                _devices = JsonConvert.DeserializeObject<List<DEV>>(json);
                System.Diagnostics.Debug.WriteLine(json);

                //MqttController.Connect();
                MqttController.Connected += (broker) => {
                    foreach (var device in _devices)
                    {
                        System.Diagnostics.Debug.WriteLine("get from MQTT");
                        broker.Subscribe(new string[] { "status/" + device.Id }, new byte[] { 0 });
                        _selected = device;
                        
                        }
                };
                Engine.CreateThread(MqttController.Connect);

                GoFirst();

            }
            return View(_devices);
        }

        static void Publish(int value)
        {
            if (MqttController.IsConnected)
            {
                MqttController.Broker.Publish(
                        "control/" + _selected.Id,
                        Encoding.ASCII.GetBytes("{\"value\":" + value + "}"));
            }
        }

                public ActionResult Status(string id, JObject o)
        {
            foreach (var device in _devices) { 
                if (device.Id == id)
                {
                    device.UpdateStatus(o.Get<int>("value"));
                    break;
                }
            }
            return Done();
        }
    }
}
