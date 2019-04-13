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
            SYS_USR_INFO bkryuser = new SYS_USR_INFO();
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
            if (Session != null && Session["userid"] != null && Session["ltype"] != null && Session["ltype"] != "" && Session["ltype"].ToString() == "2")
            {
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    bkryuser = db.SYS_USR_INFO.Find(Convert.ToInt32((Session["userid"])));
                }
               
            }
            
            return View(new Tuple<List<BKRY_ITEMS>, SYS_USR_INFO,BKRY_CREDITCARDINFO, BKRY_ORDER>(bkryList, bkryuser,null,null));
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
                    dobj.PaymentType = Convert.ToInt16(Request.Form["nm_rad_pay"]);
                    if (dobj.PaymentType ==1)
                    {
                        BKRY_CREDITCARDINFO objC = new BKRY_CREDITCARDINFO();
                        objC.OrderID = obj.Id;
                        objC.cardname = Request["Item3.cardname"].ToString();
                        objC.cardnumber = Request["Item3.cardnumber"].ToString();                        
                        objC.expyear = Request["Item3.expyear"].ToString();
                        objC.cvv = Request["Item3.cvv"].ToString();
                        objC.expmonth = " ";
                        db.BKRY_CREDITCARDINFO.Add(objC);
                    }
                    db.BKRY_DELIVERY.Add(dobj);
                    db.SaveChanges();
                    Session["Message"] = "Saved Successfully your Order# is "+ obj.Id.ToString();
                    return RedirectToAction("Index", "home");
                }
            }
            catch (Exception ex) {
                return Json(new { success = true, message = "UnSuccessfully" }, JsonRequestBehavior.AllowGet); }
        }
    }
}