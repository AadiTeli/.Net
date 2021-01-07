using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ProductManagement.Controllers
{
    public class PMController : Controller
    {
        public ActionResult Dashboard() //return Dashboard view.
        {
            return View();
        }
        // GET: PM
        DBProduct2 db = new DBProduct2(); //db is the object of Entity DBProduct2

        //GET: Index
        public ActionResult Index(string SearchBy, string Search, int? page, string sortBy, string AdvanceSearch) //Passing Argument for Search,Advance Search and Sort.
        {
            using (DBProduct2 db = new DBProduct2()) 
            {
                ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Product_Name desc" : ""; //sort by Product Name.
                ViewBag.SortCategoryParameter = string.IsNullOrEmpty(sortBy)/*sortBy == "Category" */? "Category desc" : "Category"; //sort by Category.
                var plist = db.ProductManages.AsQueryable(); //plist is variable of Queryable.
                List<ProductManage> productlist = db.ProductManages.ToList(); //productlist is variable of List.
                if (SearchBy == "Category") //Search by Category.
                {
                    plist = plist.Where(x => x.Category.StartsWith(Search) || Search == null);
                }
                else if (SearchBy == "Product_Name") //Search by Product Name.
                {
                    plist = plist.Where(x => x.Product_Name.StartsWith(Search) || Search == null);
                }
                else if (AdvanceSearch != null) //Advance Search with all attributes.
                {
                    return View(db.ProductManages.Where(x => x.Product_Name.StartsWith(AdvanceSearch) || x.Category.StartsWith(AdvanceSearch) || x.Short_Description.StartsWith(AdvanceSearch) || x.Long_Description.StartsWith(AdvanceSearch) || Search == null).ToList().ToPagedList(page ?? 1, 3));
                }
                switch (sortBy) //Switch Case For sorting.
                {
                    case "Product_Name desc": //Prooduct Name Sorted By Descending Order.
                        plist = plist.OrderByDescending(x => x.Product_Name);
                        break;
                    case "Category desc": //Category Sorted By Descending Order.
                        plist = plist.OrderByDescending(x => x.Category);
                        break;
                    case "Category": //Category Sorted By Ascending Order.
                        plist = plist.OrderBy(x => x.Category);
                        break;
                    default: //Prooduct Name Sorted By Ascending Order.
                        plist = plist.OrderBy(x => x.Product_Name);
                        break;
                }
                return View(plist.ToPagedList(page ?? 1, 5)); //Display 5 Items(Products) in Index View.
            }
        }
        //GET: Create
        public ActionResult Create() //return add new product.
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file1, HttpPostedFileBase file2, ProductManage emp) //Argument for HttpPostBase Files.
        {
            string filename1 = Path.GetFileName(file1.FileName); //filename1 variable is created for Small image.
            if (file2 != null)  //if Large image is not null then following code will execute.
            {
                string filename2 = Path.GetFileName(file2.FileName); //filename2 is variable for large image.
                string _filename2 = DateTime.Now.ToString("yymmssfff") + filename2; //Append Date Time to filename2.
                string extension2 = Path.GetExtension(file2.FileName); //Extension for filename2.
                string path2 = Path.Combine(Server.MapPath("~/Images/"), _filename2); //Create path for Large image.
                emp.Large_Image = "~/Images/" + _filename2; //Table Object emp will stores the Large image path in Images folder.
                if (extension2.ToLower() == ".jpg" || extension2.ToLower() == ".jpeg" || extension2.ToLower() == ".png" && file2.ContentLength <= 1000000) //validation for extension and size of image.
                {
                    file2.SaveAs(path2); //save file if validation is true.
                }
            }
            string _filename1 = DateTime.Now.ToString("yymmssfff") + filename1; //Append Date and time for small image.
            string extension1 = Path.GetExtension(file1.FileName); //get extension for small image.
            string path1 = Path.Combine(Server.MapPath("~/Images/"), _filename1); //Create path for Small image.
            emp.Small_Image = "~/Images/" + _filename1;//Table Object emp will stores the Small image path in Images folder.

            if (extension1.ToLower() == ".jpg" || extension1.ToLower() == ".jpeg" || extension1.ToLower() == ".png" && file1.ContentLength <= 1000000)//validation for extension and size of file.
            {
                db.ProductManages.Add(emp); 
                if (db.SaveChanges() > 0)
                {
                    file1.SaveAs(path1);
                    ViewBag.msg = "Record Added";
                    ModelState.Clear();
                }
            }
            else 
            {
                ViewBag.msg = "Size is too Large!";
            }
            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges(); //save changes in database.
            HttpCookie cookie1 = new HttpCookie("CookieAdd"); //cookie is created for Toaster message. 
            cookie1.Value = "Add"; //add value to cookie.
            Response.Cookies.Add(cookie1);  //response is stored in cookie. 
            cookie1.Expires = DateTime.Now.AddSeconds(5);  // cookie will expire in 5 seconds. 
            return RedirectToAction("Index");
        }
        //GET:Edit
        [HttpGet]
        public ActionResult Edit(int? id) //return edit view.
        {

            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ProductManage = db.ProductManages.Find(id); //ProductManage is variable of database.
            if (ProductManage != null) //if ProductManage is not null then following code is executed.
            {
                try
                {
                    if (ProductManage.Large_Image == null)
                    {
                        ProductManage.Large_Image = "true";
                        ViewBag.Large_Image = true;
                    }
                    if (ProductManage.Long_Description == null)
                    {
                        ProductManage.Long_Description = "xyz";
                        ViewBag.Long_Description = true;
                    }
                }
                catch { }
                Session["imgPath1"] = ProductManage.Small_Image;  //session for small image.
                Session["imgPath2"] = ProductManage.Large_Image;  //session for large image.
            }
            if (ProductManage == null) //If ProductManage is null then it will show Http Not Found Page.
            {
                return HttpNotFound();
            }
            return View(ProductManage);
        }
        //POST: Edit
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file1, HttpPostedFileBase file2, ProductManage emp)
        {
             if (file1 != null) //if small image is not null then following code is executed. it has same code as create.
            {
                string filename1 = Path.GetFileNameWithoutExtension(file1.FileName);
                string extension1 = Path.GetExtension(file1.FileName);
                string _filename1 = filename1 + DateTime.Now.ToString("yymmssfff")+extension1;
                string path1 = Path.Combine(Server.MapPath("~/Images/"), _filename1);
                emp.Small_Image = "~/Images/" + _filename1;
                if (extension1.ToLower() == ".jpg" || extension1.ToLower() == ".jpeg" || extension1.ToLower() == ".png" && file1.ContentLength <= 1000000)
                {
                    db.Entry(emp).State = EntityState.Modified;
                    string oldImgPath1 = Request.MapPath(Session["imgPath1"].ToString());
                    if (db.SaveChanges() > 0)
                    { file1.SaveAs(path1);
                        ModelState.Clear();
                        if (System.IO.File.Exists(oldImgPath1))
                        {
                            System.IO.File.Delete(oldImgPath1);
                        }
                    }//following code is for toaster message for 5 second.
                    HttpCookie cookie2 = new HttpCookie("CookieEdit");
                    cookie2.Value = "Edit";
                    Response.Cookies.Add(cookie2);
                    cookie2.Expires = DateTime.Now.AddSeconds(5);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Size is too Large!";
                }
            }
            else
            {
                emp.Small_Image = Session["imgPath1"].ToString();
                db.Entry(emp).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {//following code is for toaster message for 5 second.
                    HttpCookie cookie2 = new HttpCookie("CookieEdit");
                    cookie2.Value = "Edit";
                    Response.Cookies.Add(cookie2);
                    cookie2.Expires = DateTime.Now.AddSeconds(5);
                    return RedirectToAction("Index");
                }
              
            }

            if (file2 != null) //if large image is not null then following code is executed and code is same as create.
            {
                string filename2 = Path.GetFileNameWithoutExtension(file2.FileName);
                string extension2 = Path.GetExtension(file2.FileName);
                string _filename2 = filename2 + DateTime.Now.ToString("yymmssfff")+extension2;
                string path2 = Path.Combine(Server.MapPath("~/Images/"), _filename2);
                emp.Large_Image = "~/Images/" + _filename2;
                if (extension2.ToLower() == ".jpg" || extension2.ToLower() == ".jpeg" || extension2.ToLower() == ".png" && file2.ContentLength <= 1000000)
                {
                    db.Entry(emp).State = EntityState.Modified;
                    string oldImgPath2 = Request.MapPath(Session["imgPath2"].ToString());
                    if (db.SaveChanges() > 0)
                    {
                        file2.SaveAs(path2);
                        ModelState.Clear();
                        if (System.IO.File.Exists(oldImgPath2))
                        {
                            System.IO.File.Delete(oldImgPath2);
                        }
                    }//following code is for toaster message for 5 second.
                    HttpCookie cookie2 = new HttpCookie("CookieEdit");
                    cookie2.Value = "Edit";
                    Response.Cookies.Add(cookie2);
                    cookie2.Expires = DateTime.Now.AddSeconds(5);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Size is too Large!";
                }
                TempData["msg"] = "Record Updated";
            }
            else
            {
                try
                {
                    emp.Large_Image = Session["imgPath2"].ToString();
                }
                catch{ }
                db.Entry(emp).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {//following code is for toaster message for 5 second.
                    HttpCookie cookie2 = new HttpCookie("CookieEdit");
                    cookie2.Value = "Edit";
                    Response.Cookies.Add(cookie2);
                    cookie2.Expires = DateTime.Now.AddSeconds(5);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        //POST: Delete All
        [HttpPost]
            public ActionResult DeleteAll(string[] id) //id is an array for choosing multiple product to delete.
            {
            int[] getid = null; //initialize getid as null.
                if(id != null)
                {
                    getid = new int[id.Length]; //if getid is not null 
                    int j = 0;                  //initialize j 
                    foreach(string i in id)     //perform following for every i in id.
                    {
                        int.TryParse(i, out getid[j++]);
                    }
                    using (DBProduct2 db = new DBProduct2())
                    {
                        List<ProductManage> pids = new List<ProductManage>(); //pids is variable of Table ProductManage.
                        pids = db.ProductManages.Where(x => getid.Contains(x.ID)).ToList();
                        foreach(var s in pids) //it will delete all the selected products form the list.
                        {
                            db.ProductManages.Remove(s);
                        }
                        db.SaveChanges();//following code is for toaster message for 5 second.
                        HttpCookie cookie3 = new HttpCookie("CookieDeleteAll");
                        cookie3.Value = "DeleteAll";
                        Response.Cookies.Add(cookie3);
                        cookie3.Expires = DateTime.Now.AddSeconds(5);
                        return RedirectToAction("Index");
                    }
                }
            return RedirectToAction("Index");

        }
        //POST:Delete
        public ActionResult Delete(int? id)
        {
            //if id and ProductManage variable is null then it will rendered http bad request page.
            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ProductManage = db.ProductManages.Find(id); 
            if (ProductManage == null) 
            {
                return HttpNotFound();
            }
            string currentimg1 = Request.MapPath(ProductManage.Small_Image); //currentimg1 stores small image.
            string currentimg2 = Request.MapPath(ProductManage.Large_Image); //cureentimg2 stroes large image.
            db.Entry(ProductManage).State = EntityState.Deleted;
            if(db.SaveChanges()>0) //if small image & large image exists then Delete them.
            {
                if(System.IO.File.Exists(currentimg1))
                {
                    System.IO.File.Delete(currentimg1);
                }
                if (System.IO.File.Exists(currentimg2))
                {
                    System.IO.File.Delete(currentimg2);
                }
                TempData["msg"] = "Data Deleted!";
                HttpCookie cookie4 = new HttpCookie("CookieDelete");
                cookie4.Value = "Delete";
                Response.Cookies.Add(cookie4);
                cookie4.Expires = DateTime.Now.AddSeconds(5);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
