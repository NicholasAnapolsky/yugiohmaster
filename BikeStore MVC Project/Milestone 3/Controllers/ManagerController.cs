using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MileStone2A.Models;
using Milestone_3.Models;
using System.Globalization;

namespace Milestone_3.Controllers
{
    public class ManagerController : Controller
    {
        private BikeDBContext db = new BikeDBContext();
        const int DEFAULT_BIKE_CATEGORY_ID = 5;
        const int DEFAULT_GEAR_CATEGORY_ID = 8;

        public ActionResult Index()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: BikesManager
        public ActionResult BikeIndex()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var products = from x in db.Products.Include(p => p.ProductCategory).Include(p => p.ProductModel)
                           where x.ProductCategory.ParentProductCategoryID == 1 && x.SellEndDate == null
                           select x;
       
            return View(products.ToList());
        }

        public ActionResult GearIndex()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var products = from x in db.Products.Include(p => p.ProductCategory).Include(p => p.ProductModel)
                           where x.ProductCategory.ParentProductCategoryID != 1 && x.SellEndDate == null
                           select x;

            return View(products.ToList());
        }

        public ActionResult Success()
        {
            return View();
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == id
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

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
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == id
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

            Product product = db.Products.Find(id);
            product.SellEndDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Success");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: BikesManager/Details/5
        public ActionResult Details(int? id)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == id
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

            Product product = db.Products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult BikeEdit(int? id)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var BikeCategories = from x in db.ProductCategories
                                 where x.ParentProductCategoryID == 1
                                 select x;
            ViewBag.ProductCategoryID = new SelectList(BikeCategories, "ProductCategoryID", "Name", DEFAULT_BIKE_CATEGORY_ID);


            var BikeModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID == 1 &&
                              x.ProductCategory.ProductCategoryID == DEFAULT_BIKE_CATEGORY_ID
                              select new SelectListItem
                              {
                                  Value = x.ProductModel.ProductModelID.ToString(),
                                  Text = x.ProductModel.Name
                              }).Distinct();
            ViewBag.ProductModelID = BikeModels;

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == id
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BikeEdit([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product, HttpPostedFileBase picture)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var OrgSellStartDate = (from x in db.Products
                                   where x.ProductID == product.ProductID
                                   select x.SellStartDate).First();

            if (ModelState.IsValid && picture != null)
            {
                product.ThumbNailPhoto = new byte[picture.ContentLength];
                picture.InputStream.Read(product.ThumbNailPhoto, 0, picture.ContentLength);
                product.ThumbnailPhotoFileName = picture.FileName;

                product.ModifiedDate = DateTime.Now;
                product.SellStartDate = OrgSellStartDate;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.Now;
                product.SellStartDate = OrgSellStartDate;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var BikeCategories = from x in db.ProductCategories
                                 where x.ParentProductCategoryID == 1
                                 select x;
            ViewBag.ProductCategoryID = new SelectList(BikeCategories, "ProductCategoryID", "Name", DEFAULT_BIKE_CATEGORY_ID);


            var BikeModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID == 1 &&
                              x.ProductCategory.ProductCategoryID == DEFAULT_BIKE_CATEGORY_ID
                              select new SelectListItem
                              {
                                  Value = x.ProductModel.ProductModelID.ToString(),
                                  Text = x.ProductModel.Name
                              }).Distinct();
            ViewBag.ProductModelID = BikeModels;

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == product.ProductID
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult GearEdit(int? id)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var gearCategories = (from x in db.ProductCategories
                                  where x.ParentProductCategoryID != 1 && x.ProductCategoryID > 4 //the last category
                                  select new SelectListItem
                                  {
                                      Value = x.ProductCategoryID.ToString(),
                                      Text = x.Name
                                  }).Distinct();
            ViewBag.ProductCategoryID = gearCategories;

            var gearModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID != 1 &&
                              x.ProductCategory.ProductCategoryID == DEFAULT_GEAR_CATEGORY_ID
                              select new SelectListItem
                              {
                                  Value = x.ProductCategoryID.ToString(),
                                  Text = x.Name
                              }).Distinct();
            ViewBag.ProductModelID = gearModels;

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == id
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GearEdit([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product, HttpPostedFileBase picture)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var OrgSellStartDate = (from x in db.Products
                                    where x.ProductID == product.ProductID
                                    select x.SellStartDate).First();

            if (ModelState.IsValid && picture != null)
            {
                product.ThumbNailPhoto = new byte[picture.ContentLength];
                picture.InputStream.Read(product.ThumbNailPhoto, 0, picture.ContentLength);
                product.ThumbnailPhotoFileName = picture.FileName;

                product.ModifiedDate = DateTime.Now;
                product.SellStartDate = OrgSellStartDate;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.Now;
                product.SellStartDate = OrgSellStartDate;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var gearCategories = (from x in db.ProductCategories
                                  where x.ParentProductCategoryID != 1 && x.ProductCategoryID > 4 //the last category
                                  select new SelectListItem
                                  {
                                      Value = x.ProductCategoryID.ToString(),
                                      Text = x.Name
                                  }).Distinct();
            ViewBag.ProductCategoryID = gearCategories;

            var gearModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID != 1 &&
                              x.ProductCategory.ProductCategoryID == DEFAULT_GEAR_CATEGORY_ID
                              select new SelectListItem
                              {
                                  Value = x.ProductCategoryID.ToString(),
                                  Text = x.Name
                              }).Distinct();
            ViewBag.ProductModelID = gearModels;

            ViewBag.IsBike = (from x in db.Products
                              where x.ProductID == product.ProductID
                              select x.ProductCategory.ParentProductCategoryID).First() == 1;

            return View(product);
        }

        public ActionResult GearCreate()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var gearCategories = (from x in db.ProductCategories
                                  where x.ParentProductCategoryID != 1 && x.ProductCategoryID > 4 //the last category
                                  select new SelectListItem
                                  {
                                      Value = x.ProductCategoryID.ToString(),
                                      Text = x.Name
                                  }).Distinct();
            ViewBag.ProductCategoryID = gearCategories;

            var gearModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID != 1 &&
                              x.ProductCategory.ProductCategoryID == DEFAULT_GEAR_CATEGORY_ID
                              select new SelectListItem
                              {
                                  Value = x.ProductCategoryID.ToString(),
                                  Text = x.Name
                              }).Distinct();
            ViewBag.ProductModelID = gearModels;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GearCreate([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbnailPhotoFileName,ModifiedDate,Rowguid")] Product product, HttpPostedFileBase picture)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid && picture == null)
            {
                product.ModifiedDate = DateTime.Now;
                product.rowguid = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (ModelState.IsValid && picture != null)
            {
                product.ThumbNailPhoto = new byte[picture.ContentLength];
                picture.InputStream.Read(product.ThumbNailPhoto, 0, picture.ContentLength);
                product.ThumbnailPhotoFileName = picture.FileName;
                product.ModifiedDate = DateTime.Now;
                product.rowguid = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var gearCategories = (from x in db.ProductCategories
                                  where x.ParentProductCategoryID != 1 && x.ProductCategoryID > 4 //the last category
                                  select new SelectListItem
                                  {
                                      Value = x.ProductCategoryID.ToString(),
                                      Text = x.Name
                                  }).Distinct();
            ViewBag.ProductCategoryID = gearCategories;

            var gearModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID != 1 &&
                              x.ProductCategory.ProductCategoryID == DEFAULT_GEAR_CATEGORY_ID
                              select new SelectListItem
                              {
                                  Value = x.ProductCategoryID.ToString(),
                                  Text = x.Name
                              }).Distinct();
            ViewBag.ProductModelID = gearModels;

            return View(product);
        }

        public ActionResult BikeCreate()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var BikeCategories = from x in db.ProductCategories
                                 where x.ParentProductCategoryID == 1
                                 select x;
            ViewBag.ProductCategoryID = new SelectList(BikeCategories, "ProductCategoryID", "Name", DEFAULT_BIKE_CATEGORY_ID);


            var BikeModels = (from x in db.Products
                             where x.ProductCategory.ParentProductCategoryID == 1 &&
                             x.ProductCategory.ProductCategoryID == DEFAULT_BIKE_CATEGORY_ID
                              select new SelectListItem
                             {
                                 Value = x.ProductModel.ProductModelID.ToString(),
                                 Text = x.ProductModel.Name
                             }).Distinct();
            ViewBag.ProductModelID = BikeModels;

            return View();
        }

        // POST: BikesManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BikeCreate([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbnailPhotoFileName,ModifiedDate,Rowguid")] Product product, HttpPostedFileBase picture)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            
            if (ModelState.IsValid && picture != null)
            {
                product.ThumbNailPhoto = new byte[picture.ContentLength];
                picture.InputStream.Read(product.ThumbNailPhoto, 0, picture.ContentLength);
                product.ThumbnailPhotoFileName = picture.FileName;
                product.ModifiedDate = DateTime.Now;
                product.rowguid = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.Now;
                product.rowguid = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var BikeCategories = from x in db.ProductCategories
                                 where x.ParentProductCategoryID == 1
                                 select x;
            ViewBag.ProductCategoryID = new SelectList(BikeCategories, "ProductCategoryID", "Name", product.ProductCategoryID);


            var BikeModels = (from x in db.Products
                              where x.ProductCategory.ParentProductCategoryID == 1 &&
                              x.ProductCategory.ProductCategoryID == product.ProductCategoryID
                              select new SelectListItem
                              {
                                  Value = x.ProductModel.ProductModelID.ToString(),
                                  Text = x.ProductModel.Name
                              }).Distinct();
            ViewBag.ProductModelID = BikeModels;
            return View(product);
        }

        //AJAX post that returns a list of bike models by category
        [HttpPost]
        public ActionResult GetProductModelsByCategory(int BikeCategory)
        {
            var BikeModels = (from x in db.Products
                              where x.ProductCategory.ProductCategoryID == BikeCategory
                              select new SelectListItem
                              {
                                  Value = x.ProductModel.ProductModelID.ToString(),
                                  Text = x.ProductModel.Name
                              }).Distinct();

            return Json(new { BikeModelDropDown = BikeModels, Status = "OK", Error = "" });
        }

        [AllowAnonymous]
        public JsonResult UniqueProductName(string Name)
        {
            const int MORE_THAN_1_NAME = 1;
            var existingNames = (from x in db.Products
                                 where x.Name == Name
                                 select x).Count();
            var result = existingNames < MORE_THAN_1_NAME;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult UniqueProductNumber(string ProductNumber)
        {
            const int MORE_THAN_1_NAME = 1;
            var existingNames = (from x in db.Products
                                 where x.ProductNumber == ProductNumber
                                 select x).Count();
            var result = existingNames < MORE_THAN_1_NAME;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool IsLoggedIn()
        {
            return Session.Count == 0 || Session["Loggedin"].Equals("false");
        }
    }
}
