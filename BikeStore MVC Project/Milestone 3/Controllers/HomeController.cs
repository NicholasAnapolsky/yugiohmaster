using System;
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
    }
}