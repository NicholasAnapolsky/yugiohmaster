using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MileStone2A.Models;

namespace Milestone2B.Controllers
{
    public class GearController : Controller
    {
        private BikeDBContext db = new BikeDBContext();

        // GET: Gear
        public ActionResult Index()
        {
            var categories = from x in db.ProductCategories
                             where x.ProductCategoryID != 1 && x.ParentProductCategoryID == null 
                             select x;
            return View(categories.ToList());
        }

        public ActionResult Components()
        {
            var components = from x in db.ProductCategories
                             where x.ParentProductCategoryID == 2
                             select x;
            return View(components.ToList());
        }

        public ActionResult Clothing()
        {
            var clothing = from x in db.ProductCategories
                             where x.ParentProductCategoryID == 3
                             select x;
            return View(clothing.ToList());
        }

        public ActionResult Accessories()
        {
            var accesories = from x in db.ProductCategories
                           where x.ParentProductCategoryID == 4
                           select x;
            return View(accesories.ToList());
        }

        // GET: Gear/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gear = from x in db.Products
                       where x.ProductCategoryID == id && x.SellEndDate == null
                       select x;
            if (gear == null)
            {
                return HttpNotFound();
            }
            return View(gear.ToList());
        }
        
        // GET: Bikes/ProductDetails/5
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Gears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Gears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
