using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.DBModel;
using WebApp.Models;
using WebAppRegisLogin.Models;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AccountController));      //type of class

        UserEntities objUserEntities = new UserEntities();
        [HttpGet]
        public ActionResult Index()
        {
            log.Debug("Hello Welcome to the Debug Method");


            log.Warn("Warn message");


            log.Error("Error message");


            log.Fatal("Fatal message");

            return View();
        }
        public ActionResult Register()
        {
            UserModel objUserModel = new UserModel();
            return View(objUserModel);
        }

        [HttpPost]
        public ActionResult Register(UserModel objUserModel)
        {
            User objUser = new DBModel.User();
            objUser.CreatedOn = DateTime.Now;
            objUser.Email = objUserModel.Email;
            objUser.First_Name = objUserModel.FirstName;
            objUser.Last_Name = objUserModel.LastName;
            objUser.Password = objUserModel.Password;
            objUser.Phone = objUserModel.Phone;
            objUser.Age = objUserModel.Age;


            objUserEntities.Users.Add(objUser);
            objUserEntities.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Login()
        {
            LoginModel objLoginModel = new LoginModel();
            return View(objLoginModel);
        }
        [HttpPost]
        public ActionResult Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                if (objUserEntities.Users.Where(m => m.Email == objLoginModel.Email && m.Password == objLoginModel.Password).FirstOrDefault() == null)
                {
                    ModelState.AddModelError("Error", "Email & Password is Invalid");
                    return View();
                }
                else
                {
                    Session["Email"] = objLoginModel.Email;
                    return RedirectToAction("Dashboard", "Home");
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("index", "home");
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    }
    }