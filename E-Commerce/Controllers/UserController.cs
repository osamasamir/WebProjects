using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using System.Security;
using System.Web.Security;
namespace E_Commerce.Controllers
{
    public class UserController : Controller
    {
        Entities3 db = new Entities3();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User us)
        {
            Session["x"] = us.UserName;

            if (Membership.ValidateUser(us.UserName, us.UserPassword))
            {
                FormsAuthentication.SetAuthCookie(us.UserName, false);

                if (Roles.IsUserInRole(us.UserName, "admin"))
                {
                    return RedirectToAction("AddProduct", "Product");
                }
                if (Roles.IsUserInRole(us.UserName, "user"))
                {
                    return RedirectToAction("Index", "Product");

                }


            }
            TempData["z"] = "user name or password are not correct";

            return View();

        }

        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Create(User us)
        {

            if (ModelState.IsValid)
            {
                Membership.CreateUser(us.UserName, us.UserPassword,us.UserEmail);
                Roles.AddUserToRole(us.UserName, "user");
                db.User.Add(us);
                db.SaveChanges();
                return RedirectToAction("index", "User");
            }
            else
            {
                return View(us);
            }
        }

    }
}