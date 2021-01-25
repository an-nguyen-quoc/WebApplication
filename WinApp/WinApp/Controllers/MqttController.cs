using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace WinApp.Controllers
{
    class MqttController : BaseController
    {
        const string Host = "broker.emqx.io";
        const int Port = 1883;

        static MqttClient _broker;
        public static MqttClient Broker => _broker;
        public static event Action<MqttClient> Connected;
        public static event Action<MqttClient> Disconnected;

        public static bool IsConnected => _broker != null && _broker.IsConnected;
        public static void Connect()
        {
            System.Diagnostics.Debug.WriteLine("Connect to " + Host + "...");
            if (_broker == null || _broker.IsConnected == false)
            {
                try
                {
                    _broker = new MqttClient(Host, Port, false, MqttSslProtocols.None, null, null);
                    _broker.MqttMsgPublishReceived += (s, e) =>
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        var o = Newtonsoft.Json.Linq.JObject
                            .Parse(Encoding.ASCII.GetString(e.Message));

                        Engine.Execute("device/" + e.Topic, o);
                        System.Diagnostics.Debug.WriteLine(e.Topic);
                    };
                    _broker.Connect(Guid.NewGuid().ToString());
                    if (_broker.IsConnected)
                    {
                        Connected?.Invoke(_broker);
                        System.Diagnostics.Debug.WriteLine("Ket noi thanh cong");
                    }    
                }
                catch (Exception e)
                {
                }
            }
        }
        public static void Disconnect()
        {
            if (_broker != null && _broker.IsConnected)
            {
                _broker.Disconnect();
            }
        }
    }
}
