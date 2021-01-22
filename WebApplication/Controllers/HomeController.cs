using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace WepApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id)
        {
            System.Diagnostics.Debug.WriteLine("Home/Index");
            if (id != null)
            {
                var u = new UserController().Collection.FindById<Models.UserInfo>(id);
                this.User = u;

                var cookie = new HttpCookie("token", id)
                {
                    Expires = DateTime.Now.AddMinutes(10),
                };
                Response.SetCookie(cookie);
                System.Diagnostics.Debug.WriteLine(" - GoHome - ");
                return GoHome();
            }

            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult Login()
        {
            System.Diagnostics.Debug.WriteLine("Home/Login");
            return View(new Models.Account());
        }

        public ActionResult Signup()
        {
            return View(new Models.UserCreate());
        }

        [HttpPost]
        public ActionResult Signup(Models.UserCreate userCreate)
        {
            System.Diagnostics.Debug.WriteLine("Home/Signup");
            var account = GetApiObject<Models.Account>();
            account.UserName = userCreate.UserName;
            
            if (Collection.Contains(account.UserName))
            {
                ViewBag.Message = "EXISTS";
                System.Diagnostics.Debug.WriteLine("Home/Signup/Exist");
                return GoFirst(); // Chu y doan nay
                
            }
            account.Name = userCreate.Name;
            account.Email = userCreate.Email;
            account.Password = userCreate.Password;
            account.Role = Permission.USER;
            Collection.Insert(account.UserName, account);
            System.Diagnostics.Debug.WriteLine("Home/Signup/Addacc");
            return GoFirst();
        }


        public object ApiSignup(Models.UserCreate userCreate)
        {
            System.Diagnostics.Debug.WriteLine("Home/ApiSignup");
            
            //var userCreate = GetApiObject<Models.UserCreate>();
            var account = new Models.Account();

            account.UserName = userCreate.UserName;
            System.Diagnostics.Debug.WriteLine(userCreate.Email);
            if (new AccountController().Collection.Contains(account.UserName))
            {
                ViewBag.Message = "EXISTS";
                System.Diagnostics.Debug.WriteLine("Home/Signup/Exist");
                return Error(-1); 
                 // Chu y doan nay

            }
            if (!userCreate.Password.Equals(userCreate.ConfirmPassword))
            {
                ViewBag.Message = "PASSWORD NOT MATCHING";
                System.Diagnostics.Debug.WriteLine("Home/Signup/PASS NOT MATCH");
                return Error(-2);
            }    
            account.Password = MD5Hash(userCreate.Password);
            account.Email = userCreate.Email;
            account.Name = userCreate.Name;
            account.Role = Permission.USER;
            new AccountController().Collection.Insert(account.UserName, account);
            System.Diagnostics.Debug.WriteLine("Home/Signup/Addacc");
            return Success();
            }

        public ActionResult Logout()
        {
            new UserController().Collection.Delete(User.Id);
            Session.Abandon();
            return GoFirst();
        }

        public object ApiLogin()
        {
            System.Diagnostics.Debug.WriteLine("Home/ApiLogin");
            var acc = GetApiObject<Models.Account>();
            var id = acc.UserName.ToLower();
            var e = new AccountController()
                .Collection
                .FindById<Models.Account>(id);

            if (e == null) { return Error(-1); }
            if (e.Password != MD5Hash(acc.Password)) { return Error(-2); }

            var token = MD5Hash(id + DateTime.Now);
            var u = new Models.UserInfo
            {
                Account = e,
                Name = e.Name,
            };

            new UserController().Collection.Insert(token, u);
            
            u.Id = token;
            System.Diagnostics.Debug.WriteLine("Home/ApiLogin/Success");
            return Success(u);
        }

        [HttpPost]
        public ActionResult Login(Models.Account account)
        {
            System.Diagnostics.Debug.WriteLine("Home/Login(acc)");
            var e = new AccountController()
                .Collection
                .FindById<Models.Account>(account.UserName);

            var msg = "UN";
            if (e != null)
            {
                msg = "PW";
                if (e.Password == account.Password)
                {
                    User = new Models.UserInfo
                    {
                        Account = e,
                        Name = account.UserName,
                    };
                    return Redirect("/" + e.Role);
                }
            }
            ViewBag.Message = msg;
            return View(account);
        }
    }
}
