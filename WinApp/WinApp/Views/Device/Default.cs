using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Windows.Forms;
namespace WinApp.Views.Device
{
    class Default : BaseView<List<DeviceViewModel>, FlowLayoutPanel>
    {
        //static DeviceViewModel device_;
        protected override void RenderBody()
        {
            foreach (var device in Model)
            {
                var view = new DeviceView(device);
                 
                view.Click += new EventHandler(Device_click);
                MainContent.Controls.Add(view);
            }

            MainContent.Dock = DockStyle.Fill;
        }

        void Device_click(object sender, System.EventArgs e)
        {
            DeviceView clicked = (DeviceView)sender;
            MainContent.Controls.Add(new Device.UpdateStatus(clicked._selected));
        }
    }
}
