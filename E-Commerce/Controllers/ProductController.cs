using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using System.IO;

namespace E_Commerce.Controllers
{
    
    public class ProductController : Controller
    {
        Entities3 db=new Entities3();
        // GET: Product
        [Authorize(Roles ="user")]
        public ActionResult Index()
        {
            var data = db.Product.AsNoTracking().ToList();

            return View(data);
        }
        public ActionResult Cart(int id, string username)
        {
            User User = db.User.Where(a => a.UserName == username).FirstOrDefault();
            Product Product = db.Product.Find(id);
            Product.UserId = User.UserId;
            db.SaveChanges();
            Session["totalprice"] = db.Product.Where(a => a.UserId == User.UserId).Sum(a => a.ProductPrice);
            return View(db.Product.Where(a => a.UserId == User.UserId).ToList());
        }
        public ActionResult EmptyCart(string username)
        {
            User User = db.User.Where(a => a.UserName == username).FirstOrDefault();
            List<Product> Products = db.Product.Where(a => a.UserId == User.UserId).ToList();
            foreach (var product in Products)
            {
                product.UserId = null;
            }
            db.SaveChanges();
            return View();
        }
        public ActionResult CartAjax(int id, string username)
        {
            User User = db.User.Where(a => a.UserName == username).FirstOrDefault();
            Product Product = db.Product.Find(id);
            Product.UserId = User.UserId;
            db.SaveChanges();
            Session["totalprice"] = db.Product.Where(a => a.UserId == User.UserId).Sum(a => a.ProductPrice);
            return View();
        }
        public ActionResult ViewCart(string username)
        {
            User User = db.User.Where(a => a.UserName == username).FirstOrDefault();
            Session["totalprice"] = db.Product.Where(a => a.UserId == User.UserId).Sum(a => a.ProductPrice);
            return View(db.Product.Where(a => a.UserId == User.UserId).ToList());
        }
        [Authorize(Roles = "admin")]

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product p, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                string photopath = Server.MapPath("~/docs/");
                string photoname = Guid.NewGuid() + Path.GetFileName(photo.FileName);
                string finalpath = Path.Combine(photopath, photoname);
                photo.SaveAs(finalpath);
                p.ProductImage = photoname;
                db.Product.Add(p);
                db.SaveChanges();
                return RedirectToAction("index", "Product");
            }
            else
            {
                return View(p);

            }
        }

    }
}