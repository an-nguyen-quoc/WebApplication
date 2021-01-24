using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;
using WinApp.Controllers;

namespace WinApp.Views
{
    class Menu : Button  
    {
        public string url { get; set; }
        public bool click { get; set; } = false;
        public Menu(Env.Menu menu)
        {
            //this.FlatStyle = FlatStyle.Popup;
            this.Text = menu.Name;
            this.BackColor = Color.White;
            this.url = menu.URL;
            this.click = false;
            //this.Controls.Add();
            //int w = 0;
            //int x = 0, y = 10;

            //foreach (var p in menu.Menuitems)
            //{
            //    //var led = new Led { Name = p.Key, State = p.Value };
            //    //this.Controls.Add();

            //    if (w == 0)
            //    {
            //        w = led.Width;
            //        y += w >> 1;
            //        x = w;
            //    }
            //    led.Left = x + (w >> 1);
            //    led.Top = y;
//
            //    x += w << 1;
            //}
            //this.Height = (w << 1) + y - 10;
            //this.Width = x + (w >> 1);

            //device.Changed += (d, v) => {
            //    foreach (var p in d.Status)
            //    {
            //        var led = (Led)this.Controls[p.Key];
            //        led.State = p.Value;
            //    }
            //};
            this.Click += new EventHandler(this.Btn_click);
        }
        
        void Btn_click (object sender, System.EventArgs e)
        {
            Menu clickedButton = (Menu)sender;
            clickedButton.click = true;
            MenuView.menuRedirect(clickedButton.url, clickedButton.click);
            //clickedButton.ủ
        }

        protected void Redirect(string url, params object[] values)
        {
            System.Mvc.Engine.Execute(url, values);
        }
    }
}
