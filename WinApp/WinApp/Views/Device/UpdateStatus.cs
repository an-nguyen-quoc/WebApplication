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
            if (MqttController.IsConnected)
            {
                MqttController.Broker.Publish(
                        "control/" + device.Id,
                        Encoding.ASCII.GetBytes("{\"value\":" + value + "}"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
