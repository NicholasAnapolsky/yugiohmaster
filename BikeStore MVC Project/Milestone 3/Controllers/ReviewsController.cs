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
    public class ReviewsController : Controller
    {
        private BikeDBContext db = new BikeDBContext();

        // GET: Reviews
        public ActionResult Index()
        {
            return View(db.Reviews.ToList());
        }

        public ActionResult AddReview(int? id)
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
            Reviews review = new Reviews();
            review.ProductID = product.ProductID;
            return View(review);
        }

        public ActionResult getReviews(int? id)
        {
            Product product = db.Products.Find(id);
            var productReview = from x in db.Reviews where product.ProductID == x.ProductID select x;
            var reviews = productReview.ToList();
            ViewBag.Count = reviews.Count;

            return PartialView(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview([Bind(Include = "id,Name,ProductID,Review,Rating")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                if (reviews.Name == null || reviews.Name == "" || reviews.Name == " ")
                {
                    reviews.Name = "Anonymous";
                }
                db.Reviews.Add(reviews);
                db.SaveChanges();
                return RedirectToAction("ReviewSuccess");
            }

            return View(reviews);
        }

        public ActionResult ReviewSuccess()
        {
            return View();
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
