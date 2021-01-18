using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;


namespace WepApp.Controllers
{
    public class AccountController : BaseAdminController
    {
        // GET: Account
        public ActionResult Index()
        {
            var u = User;
            if (u == null)
            {
                return Redirect("/home");
            }
            if (u.Account.Role <= Permission.USER)
                return Redirect("/user");
            System.Diagnostics.Debug.WriteLine("Account/Index");
            return View(Collection.ToList<Models.Account>());
        }
        public object InsertAccount(Models.Account acc)
        {
            System.Diagnostics.Debug.WriteLine("Account/InsertAccount(acc)");
            var id = acc.Id.ToLower();
            if (Collection.Contains(id))
            {
                return null;
            }

            acc.Password = MD5Hash(acc.Password);
            Collection.Insert(id, acc);
            return id;
        }
        public override object ApiInsert()
        {
            System.Diagnostics.Debug.WriteLine("Account/ApiInsert");
            var acc = GetApiObject<Models.Account>();
            var id = InsertAccount(acc);
            if (id == null)
            {
                return Error(-1);
            }
            return Success(acc);
        }
        public ActionResult Create()
        {
            System.Diagnostics.Debug.WriteLine("Account/Create");
            return View(new Models.AccountBinding());
        }

        [HttpPost]
        public ActionResult Create(Models.AccountBinding account)
        {
            System.Diagnostics.Debug.WriteLine("Account/Create(acc)");
            if (Collection.Contains(account.UserName))
            {
                ViewBag.Message = "EXISTS";
                return View(account);
            }
            account.Role = Models.Permission.USER;
            Collection.Insert(account.UserName, account);
            return GoFirst();
        }
    }
}