using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index() //return Index View
        {
            return View();
        } 
        //GET:Registration
        [HttpGet]
        public ActionResult UserRegistration(int id = 0) //return Registration View
        {
            Registration reg = new Registration(); //Object of Registration Table

            return View(reg);
        }
        //POST:Registration
        [HttpPost]
        public ActionResult UserRegistration(Registration reg) //Whenever User Click on Register button, it will redirect here  
        {
            using (DBProduct DB = new DBProduct()) //DB is the object of Entitiy DBProduct
            {                
                    if (DB.Registrations.Any(x => x.UserName == reg.UserName)) //Condition for User Where they already exists or not.
                    {      
                    ViewBag.DuplicateMessage = "UserName Already Exist.";
                    return View("UserRegistration",reg);
                    }
                    DB.Registrations.Add(reg); //It stores the data into database.
                    DB.SaveChanges(); //Changes are saved in database.
            }
            ModelState.Clear();
            HttpCookie cookie2 = new HttpCookie("CookieReg"); //cookie is created for Toaster message. 
            cookie2.Value = "Reg"; //add value to cookie.
            Response.Cookies.Add(cookie2);  //response is stored in cookie. 
            cookie2.Expires = DateTime.Now.AddSeconds(2);
            return View("UserLogin"); //It will redirect to Login Page.
            
        }
        //GET:Login
        [HttpGet]
        public ActionResult UserLogin(int id=0) //return Login View.
        {
            Login log = new Login(); // Log is the Object of Login Class. 
            return View(log);
        }
        [HttpPost]
        public ActionResult UserLogin(Login log) //Whenever User Click on Login button, it will redirect here
        {
            using (DBProduct DB = new DBProduct())
            {
                if (ModelState.IsValid) // It will perform Validation 
                {
                    if (DB.Registrations.Where(m => m.Email == log.Email && m.Password == log.Password).FirstOrDefault() == null) //Check for Email and Password
                    {
                        ModelState.AddModelError("Error", "Invalid Email or Password!");
                        return View();
                    }
                    else
                    {
                        Session["UserName"] = ((DB.Registrations.Where(m => m.Email == log.Email && m.Password == log.Password).FirstOrDefault()).UserName).ToString(); 
                        return RedirectToAction("Dashboard", "PM"); //It will redirect to Dashboard after suceessfully Login.
                    }
                }
            }
            return View(); 
        }
        //GET: Logout
        [HttpPost]
        public ActionResult Logout() //Whenever User Click on Logout button, it will redirect here
        {
            Session.Abandon(); //session destroyed
            return RedirectToAction("UserLogin","Product"); //It will redirect to registration page.
        }
    }
}