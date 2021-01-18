using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
namespace WepApp.Controllers
{
    public class BaseAdminController : BaseController
    {

        public override Permission Role => Permission.ADMIN;
    }
    public class AdminController : BaseAdminController
    {
        // GET: Admin
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Admin/Index");
            return Redirect("/account");
        }
    }
}