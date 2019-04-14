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
    }
}