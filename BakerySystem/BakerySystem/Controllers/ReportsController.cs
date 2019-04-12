using BakerySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetReport(string nm_rpttype,string nm_dtfrom,string nm_dtTo)
        {
            List<BKRY_ORDER> obh = new List<BKRY_ORDER>();
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                
                switch (nm_rpttype)
                {
                    case "1":
                        List<LoginDetail> LoginDetailList = db.LoginDetails.ToList<LoginDetail>();
                        List<LoginDetail> LoginDetailList2 = new List<LoginDetail>();
                        LoginDetail item = new LoginDetail();
                        LoginDetailList = LoginDetailList.Where(x=>x.LoginTime >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.LoginTime <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo).AddDays(1).AddTicks(-1) : DateTime.Now)).ToList();
                        foreach (var item2 in LoginDetailList)
                        {
                            item2.add_dtee = Convert.ToDateTime(item2.LoginTime).ToLongDateString();
                            item = item2;
                            LoginDetailList2.Add(item);
                        }
                        return Json(new { data = LoginDetailList2 }, JsonRequestBehavior.AllowGet);                       
                    case "2":                        
                        var BKRY_ORDERList = db.BKRY_DELIVERY.Join(db.BKRY_ORDER,a=>a.OrderId,b=>b.Id, (a, b) => new { a, b }).Select(s => new BKRY_ORDER2
                        {
                            OrderId = ((int)s.a.OrderId).ToString(),
                            personname = s.b.personname,
                            address = s.b.address,
                            postCode = s.b.postCode,
                            Orderon = (DateTime)s.b.inserted_dt,
                            Delivered = s.a.DeliveryStatus == 0 ? "No" : s.a.DeliveryStatus == 1 ? "Dispatched" : s.a.DeliveryStatus == 2 ? "Near By" : "Yes",
                            PaymentType = s.a.PaymentType == 0 ? "Cash on Delivery" : "Credit Card Payment"
                        }).ToList().Where(y => y.Orderon >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && y.Orderon <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo).AddDays(1).AddTicks(-1) : DateTime.Now)) ;
                        foreach (BKRY_ORDER2 item3 in BKRY_ORDERList)
                        {
                            item3.add_dtee = Convert.ToDateTime(item3.Orderon).ToLongDateString();
                        }
                        return Json(new { data = BKRY_ORDERList.ToList<BKRY_ORDER2>() }, JsonRequestBehavior.AllowGet);
                        
                    case "3":
                        List<BKRY_ITEMS> bkryList = db.BKRY_ITEMS.ToList<BKRY_ITEMS>();
                        bkryList = bkryList.Where(x => x.add_date >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.add_date <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo).AddDays(1).AddTicks(-1) : DateTime.Now)).ToList<BKRY_ITEMS>();
                        foreach (BKRY_ITEMS item3 in bkryList)
                        {
                            item3.image = null;
                            item3.add_dtee = Convert.ToDateTime(item3.expiry_dte).ToLongDateString();                            
                        }
                        return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);
                    default:
                        break;
                }
                return null;
            }
        }

        }
    
}