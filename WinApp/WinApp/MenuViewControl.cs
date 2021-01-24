using Newtonsoft.Json;
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

namespace WinApp
{
    public partial class MenuViewControl : UserControl
    {
        public MenuViewControl()
        {
            InitializeComponent();
            string workingDirectory = Environment.CurrentDirectory;
            string currentpath = Directory.GetParent(workingDirectory).Parent.FullName;
            string filepath = currentpath + @"\menuconfig.json";
            
            System.Diagnostics.Debug.WriteLine(filepath);
            List<Controllers.Env.Menu> menulist = JsonConvert.DeserializeObject<List<Controllers.Env.Menu>>(File.ReadAllText(filepath));
            System.Diagnostics.Debug.WriteLine(menulist);
            int i = 0;
            foreach (var p in menulist)
            {
                var menu_ = new Views.Menu(p);
                //menu_.Height = 30;
                //menu_.Width. = 100;
                //menu_.po
                menu_.AutoSize = true;

                //menu_.Top = menu_.Height * i;
                menu_.Font = new Font("Segoe UI", 18);
                i++;
                menu_.Dock = DockStyle.Top;
                this.Controls.Add(menu_);

                //this.Dock = DockStyle.Fill;
                
            }
            //this.Dock = DockStyle.Fill;

        }
    }
}
