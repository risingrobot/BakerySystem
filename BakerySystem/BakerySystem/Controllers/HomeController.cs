using BakerySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class HomeController : Controller
    {
        BKRY_MNGT_SYSEntities db = null;
        public HomeController()
        {
            db = new BKRY_MNGT_SYSEntities();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult getImage(int id)
        {
            BKRY_CATEGORY obj = null;
            obj = db.BKRY_CATEGORY.Where(x => x.Id == id).ToList().FirstOrDefault();
            if (obj == null || obj.image == null)
            {
                return null;
            }
            return File(obj.image, "image/jpeg"); // Might need to adjust the content type based on your actual image type            

        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult getImage2(int id)
        {
            BKRY_ITEMS obj = null;
            obj = db.BKRY_ITEMS.Where(x => x.Id == id).ToList().FirstOrDefault();
            if (obj == null || obj.image == null)
            {
                return null;
            }
            return File(obj.image, "image/jpeg"); // Might need to adjust the content type based on your actual image type            

        }
        public ActionResult ClearMessage()
        {
            Session["Message"] = null;
            Session["Order"] = null;
            return null;
        }
        public ActionResult aboutus()
        {
            return View();
        }
        public ActionResult contactus()
        {
            return View();
        }
        public ActionResult Cust_FeedBack()
        {
            return PartialView("~/Views/Shared/Cust_FeedBack.cshtml");
        }
        public ActionResult CreateFeedback()
        {
            using (db = new BKRY_MNGT_SYSEntities())
            {
                CUST_FEED obj = new CUST_FEED();
                obj.Customer_ID = null;
                obj.Order_id = Session["Order"].ToString();
                obj.feedback = Request.Form["feedback"].ToString();
                obj.Creation_Id = DateTime.Now.ToShortDateString();
                db.CUST_FEED.Add(obj);
                db.SaveChanges();
            }
            Session["Message"] = null;
            Session["Order"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Cfeedback()
        {
            List<CUST_FEED> obj = new List<CUST_FEED>();
            using (db = new BKRY_MNGT_SYSEntities())
            {
                obj = db.CUST_FEED.OrderByDescending(x=>x.Feedback_Id).ToList();
            }
            return View(obj);
        }
        public ActionResult Helper()
        {
            return View("~/Views/Home/Helper.cshtml");
        }
    }
}