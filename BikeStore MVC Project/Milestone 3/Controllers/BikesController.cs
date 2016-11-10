using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MileStone2A.Models;

namespace MileStone2A.Controllers
{
    public class BikesController : Controller
    {
        private BikeDBContext db = new BikeDBContext();

        // GET: Bikes
        public ActionResult Index()
        {
            var categories = from x in db.ProductCategories
                             where x.ParentProductCategoryID == 1
                             select x;

            return View(categories.ToList());
        }

        public ActionResult Index2(string searchString)
        {
            var categories = from x in db.Products select x;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.Name.Contains(searchString));
            }

            return View(categories.ToList());
        }
        public ActionResult Mountain()
        {
            var mountain = from x in db.ProductAndDescription
                           where x.Culture == "en" && x.ProductCategoryID == 5
                           select new BikeListModel()
                           {
                               ProductModel = x.ProductModel,
                               Description = x.Description,
                               ProductModelID = x.ProductModelID
                           };
            return View(mountain);
        }

        public ActionResult Road()
        {
            //var road = from x in db.ProductAndDescription where x.Culture == "en" && x.ProductCategoryID == 6 select x;
            var road = from x in db.ProductAndDescription
                       where x.Culture == "en" && x.ProductCategoryID == 6
                       select new BikeListModel()
                       {
                           ProductModel = x.ProductModel,
                           Description = x.Description,
                           ProductModelID = x.ProductModelID
                       };
            return View(road);
        }

        public ActionResult Touring()
        {
            var touring = from x in db.ProductAndDescription
                           where x.Culture == "en" && x.ProductCategoryID == 7
                           select new BikeListModel()
                           {
                               ProductModel = x.ProductModel,
                               Description = x.Description,
                               ProductModelID = x.ProductModelID
                           };
            return View(touring);
        }

        // GET: Bikes/Details/5
        public ActionResult Details(int? id, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ProductDescription = from x in db.Products
                                     where x.ProductModelID == id
                                     select x;

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductDescription = from x in db.Products
                                     where x.ProductModelID == id && x.Name.Contains(searchString)
                                     select x;
                //categories = categories.Where(s => s.Name.Contains(searchString));
            }

            

            if (ProductDescription == null)
            {
                return HttpNotFound();
            }
            return View(ProductDescription);
        }
    }
}