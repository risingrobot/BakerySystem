using BakerySystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            List<BKRY_ITEMS> bkryList = new List<BKRY_ITEMS>();
            if (Session != null && Session["Cart"] != null && ((List<int>)Session["Cart"]).Count > 0)
            {
                List<int> itemids = ((List<int>)Session["Cart"]);

                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    bkryList = db.BKRY_ITEMS.Where(x => itemids.Contains(x.Id)).ToList<BKRY_ITEMS>();
                    foreach (BKRY_ITEMS item in bkryList)
                    {
                        item.image = null;
                        item.quantity = itemids.GroupBy(i => i).Where(x => x.Key == item.Id).FirstOrDefault().Count();
                    }                   
                }

            }
            
            return View(bkryList);
        }
        public ActionResult CreateOrder()
        {
            List<BKRY_ITEMS> bkryList = new List<BKRY_ITEMS>();
            if (Session != null && Session["Cart"] != null && ((List<int>)Session["Cart"]).Count > 0)
            {
                List<int> itemids = ((List<int>)Session["Cart"]);

                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    bkryList = db.BKRY_ITEMS.Where(x => itemids.Contains(x.Id)).ToList<BKRY_ITEMS>();
                    foreach (BKRY_ITEMS item in bkryList)
                    {
                        item.image = null;
                        item.quantity = itemids.GroupBy(i => i).Where(x => x.Key == item.Id).FirstOrDefault().Count();
                    }
                }

            }
            try
            {
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    BKRY_ORDER obj = new BKRY_ORDER();
                    obj.OrderDetails = JsonConvert.SerializeObject(bkryList);
                    obj.personname = Request["name"].ToString();
                    obj.phone = Request["phone"].ToString();
                    obj.email = Request["email"].ToString();
                    obj.address = Request["address"].ToString();
                    obj.street = Request["street"].ToString();
                    obj.postCode = Request["postcode"].ToString();
                    obj.inserted_dt = DateTime.Now;

                    db.BKRY_ORDER.Add(obj);
                    db.SaveChanges();
                    Session["Cart"] = null;
                    BKRY_DELIVERY dobj = new BKRY_DELIVERY();
                    dobj.OrderDetails = JsonConvert.SerializeObject(bkryList);
                    dobj.OrderId = obj.Id;
                    dobj.inserted_dt = DateTime.Now;
                    dobj.DeliveryStatus = 0;

                    db.BKRY_DELIVERY.Add(dobj);
                    db.SaveChanges();
                    Session["Message"] = "Saved Successfully your Order# is "+ obj.Id.ToString();
                    return RedirectToAction("Index", "home");
                }
            }
            catch (Exception) { return Json(new { success = true, message = "UnSuccessfully" }, JsonRequestBehavior.AllowGet); }
        }
    }
}