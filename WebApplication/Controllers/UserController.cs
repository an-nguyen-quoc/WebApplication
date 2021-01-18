using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace WepApp.Controllers
{
    public class UserController : BaseController
    {
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }
    }
}