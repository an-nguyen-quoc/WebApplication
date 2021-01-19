using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Models;

namespace WepApp.Controllers
{
    public class UserController : BaseController
    {
        public System.Web.Mvc.ActionResult Index()
        {
            var u = User;
            if (u == null)
            {
                return Redirect("/home");
            }
             
            System.Diagnostics.Debug.WriteLine("User/Index");
            var lst = new List<Models.Account>();
            lst.Add(u.Account);
            return View(lst);
        }
    }
}