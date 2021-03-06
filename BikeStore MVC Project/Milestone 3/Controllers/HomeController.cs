﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MileStone2A.Models;
using CaptchaMvc.HtmlHelpers;
using Milestone_3.Models;
using System.Net.Mail;
using System.Net;

namespace Milestone2B.Controllers
{
    public class HomeController : Controller
    {
        private BikeDBContext db = new BikeDBContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Index2()
        {
            var categories = from x in db.Products select x;

            return View(categories.ToList());
        }

        [HttpPost]
        public ActionResult Index2(string SearchString)
        {
            var categories = from x in db.Products select x;

            if (!String.IsNullOrEmpty(SearchString))
            {
                categories = categories.Where(s => s.Name.Contains(SearchString));
            }
            return View(categories.ToList());
        }


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

        public ActionResult Contact()
        {
            Contact contact = new Contact();
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact contact)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("ContactResult", new { status = SendEmail(contact) } );
                }
            } else
            {
                ViewBag.captchaError = "Error: captcha is not valid.";
            }

            return View(contact);
        }

        public string SendEmail(Contact contact)
        {
            string successMessage = "Success! Your message has been sent.| Now enjoy buying our products!";
            string errorMessage = "Error! There was an issue, your message was not sent.| Unfortunatly we're not liable for this error, so please contact your nearest tissue box!";

            try
            {
                string to = "barsoflarsmars@gmail.com";
                string password = "marsbars";
                string from = contact.Email;
                string fromName = contact.FirstName + " " + contact.LastName;
                string toName = "lars bars";
                string subject = "Contacted Us on " + DateTime.Now.ToString("MMMM d, yyyy h:m:s tt");
                string body =
                    "First Name: " + contact.FirstName +
                    "\nLast Name: " + contact.LastName +
                    "\nEmail: " + contact.Email +
                    "\nComment:\n" + contact.Comment +
                    "\n";

                MailMessage msg = new MailMessage(new MailAddress(from, fromName), new MailAddress(to, toName));
                msg.Subject = subject;
                msg.Body = body;

                SmtpClient smtp = new SmtpClient();
                //TODO: Setup Message
                //Add a Reply-to the message having the inputted user's email

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(to, password);

                smtp.Send(msg);
                msg.Dispose();

                return successMessage;
            }
            catch (Exception)
            {
                return errorMessage;
            }
        }

        public ActionResult ContactResult(string status)
        {
            ViewBag.Status = status;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult _Login()
        {            
            return PartialView(new Managers());
        }

        [HttpPost]
        public ActionResult ManageLogin(string Email, string Password)
        {
            
            var managerQuery = from x in db.Managers
                               where x.Email == Email &&
                               x.Password == Password
                               select x;
            if ((string)Session["LoggedIn"] == "true")
            {
                Session["LoggedIn"] = "false";
                
                return Json(new { ManagerModel = new Managers(), Status = "OK", Error = "" });
            }
            else
            {
                if (managerQuery.Count() > 0)
                {
                    var dbManagers = managerQuery.First();
                    if (ModelState.IsValid)
                    {
                        Session["LoggedIn"] = "true";
                        Session["User"] = dbManagers.FirstName;
                        return Json(new { ManagerModel = dbManagers, Status = "OK", Error = "" });
                    }
                }
                else
                {
                    Session["LoginError"] = "This manager does not exist.";
                }
            }

            Session["LoggedIn"] = "false";

            return Json(new { ManagerModel = new Managers(), Status = "OK", Error = "" });
        }

        public ActionResult _Cart()
        {
            if (Session.Count == 0 && Session["Cart"] == null)
            {
                Session["Cart"] = new List<Cart>();
                Session["Total"] = 0;
            }           
            return PartialView();
        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            Session["Cart"] = new List<Cart>();
            Session["Total"] = 0;
            return Json(new { Status = "OK", Error = "" });
        }

        [HttpPost]
        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session.Count == 0 && Session["Cart"] == null)
            {
                Session["Cart"] = new List<Cart>();
                Session["Total"] = 0;
            }

            Product product = db.Products.Find(id);

            var curCart = (List<Cart>)Session["Cart"];
            curCart.Add(new Cart(product.ProductID, product.Name, product.Color, product.ListPrice, product.Size, product.Weight));
            Session["Cart"] = curCart;
            Session["Total"] = product.ListPrice + Convert.ToDecimal(Session["Total"]);

            return Json(new { Status = "OK", Error = "" });
        }

        [HttpPost]
        public ActionResult RemFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session.Count == 0 && Session["Cart"] == null)
            {
                Session["Cart"] = new List<Cart>();
                Session["Total"] = 0;
            }

            var curCart = (List<Cart>)Session["Cart"];

            Cart remItem = curCart.Find(x => x.productID == id);

            curCart.Remove(remItem);
            Session["Cart"] = curCart;
            Session["Total"] = Convert.ToDecimal(Session["Total"]) - remItem.Price;

            return Json(new { Status = "OK", Error = "" });
        }

        [HttpPost]
        public ActionResult Checkout()
        {
            if (Convert.ToDecimal(Session["Total"]) != 0)
            {
                Session["Cart"] = new List<Cart>();
                Session["Total"] = 0;

                return Json(new { Status = "Checkout", Error = "" });
            }
            else
            {
                return Json(new { Status = "Error", Error = "Add products to the cart before checking out." });
            }
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}