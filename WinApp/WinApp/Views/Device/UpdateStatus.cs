using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Controllers;

namespace WinApp.Views.Device
{
    public partial class UpdateStatus : UserControl
    {
        static DeviceViewModel selectedDevice;
        public UpdateStatus(DeviceViewModel device)
        {
            InitializeComponent();

            selectedDevice = device;
            this.textBox1.Text = device.Id.ToString();
        }

        static void Publish(DeviceViewModel device,  int value)
        {
            //if (!MqttController.IsConnected)
             //   MqttController.Connect();
            if (MqttController.IsConnected)
            {
                MqttController.Broker.Publish(
                        "controller/" + device.Id,
                        Encoding.ASCII.GetBytes("{\"value\":" + value + "}"));
                System.Diagnostics.Debug.WriteLine(device.Id);
                System.Diagnostics.Debug.WriteLine(value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(this.textBox2.Text);
            value = value & 15;
            Publish(selectedDevice, value);
            Engine.CreateThread(MqttController.Connect);
        }
    }
}
