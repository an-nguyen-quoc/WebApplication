using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Controllers;
namespace WinApp.Views
{
    class MenuView : BaseView<List<Env.Menu>, FlowLayoutPanel>
    {
        public MenuView(List<Env.Menu> menuList)
        {
            foreach (var _menu in menuList)
            {
                //this.Container.Add(new Menu(_menu));
                MainContent.Controls.Add(new Menu(_menu));
            }    
        }

        public static void menuRedirect(string url, bool click)
        {
            if (click)
                System.Mvc.Engine.Execute(url);
        }
    }
}
