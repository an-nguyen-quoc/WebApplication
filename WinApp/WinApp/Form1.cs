﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTT = WinApp.Controllers.MqttController;

namespace WinApp
{
    public partial class Form1 : Form
    {
        delegate void UpdateMainContent(Views.IWindowView view);

        Views.IWindowView _currentView;
        public Form1()
        {
            InitializeComponent();
            //MQTT.Connect();
            Engine.ValidateActionResult = result =>
            {
                var form = result.View as Views.IPopup;
                if (form != null)
                {
                    form.Show();
                    return;
                }
                var callback = new UpdateMainContent(SetMainContent);
                this.BeginInvoke(callback, new object[] { result.View as Views.IWindowView });
            };
        }

        void SetMainContent(Views.IWindowView view)
        {
            var menuview = new MenuViewControl();
            //menuview.Dock = DockStyle.Fill;
            flowLayoutPanel2.Controls.Add(menuview);
            //flowLayoutPanel2.Dock = DockStyle.Fill;
            //panel3.Controls.Add(new Views.MenuView(menulist));
            if (view == null) return;

            var content = view.GetMainContent();
            if (content != null)
            {
                panel1.Controls.Add(content);

                if (_currentView != null)
                {
                    if (_currentView is IDisposable)
                    {
                        ((IDisposable)(_currentView)).Dispose();
                    }
                    panel1.Controls.RemoveAt(0);
                }
                _currentView = view;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            MQTT.Connected += (br) => {
                led1.State = 1;
            };
            
            Engine.Execute("home/login");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Engine.Execute("home/exit");
            base.OnClosing(e);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
