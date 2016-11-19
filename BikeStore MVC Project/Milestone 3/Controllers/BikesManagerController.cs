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

namespace Milestone_3.Controllers
{
    public class BikesManagerController : Controller
    {
        private BikeDBContext db = new BikeDBContext();
        const int DEFAULT_BIKE_CATEGORY_ID = 5;

        // GET: BikesManager
        public ActionResult Index()
        {
            var products = from x in db.Products.Include(p => p.ProductCategory).Include(p => p.ProductModel)
                           where x.ProductCategory.ParentProductCategoryID == 1
                           select x;
       
            return View(products.ToList());
        }

        // GET: BikesManager/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewBag.ProductModelID = new SelectList(db.ProductModel, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.Now;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewBag.ProductModelID = new SelectList(db.ProductModel, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        public ActionResult Create()
        {
            
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
            /*
            if (Session.Count == 0 || Session["Loggedin"].Equals("false"))
            {
                return RedirectToAction("Index", "Home");
            }
            */
            return View();
        }

        // POST: BikesManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbnailPhotoFileName,ModifiedDate,Rowguid")] Product product, HttpPostedFileBase picture)
        {
            product.ThumbNailPhoto = new byte[picture.ContentLength];
            if (ModelState.IsValid)
            {
                picture.InputStream.Read(product.ThumbNailPhoto, 0, picture.ContentLength);
                product.ThumbnailPhotoFileName = picture.FileName;
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
                              where x.ProductCategory.ParentProductCategoryID == 1 &&
                              x.ProductCategory.ProductCategoryID == BikeCategory
                              select new SelectListItem
                              {
                                  Value = x.ProductModel.ProductModelID.ToString(),
                                  Text = x.ProductModel.Name
                              }).Distinct();

            return Json(new { BikeModelDropDown = BikeModels, Status = "OK", Error = "" });
        }

        public JsonResult UniqueProductName(string Name)
        {
            const int MORE_THAN_1_NAME = 1;
            var existingNames = (from x in db.Products
                         where x.Name == Name
                         select x).Count();
            var result = existingNames < MORE_THAN_1_NAME;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UniqueProductNumber(string ProductNumber)
        {
            const int MORE_THAN_1_NAME = 1;
            var existingNames = (from x in db.Products
                                 where x.ProductNumber == ProductNumber
                                 select x).Count();
            var result = existingNames < MORE_THAN_1_NAME;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
