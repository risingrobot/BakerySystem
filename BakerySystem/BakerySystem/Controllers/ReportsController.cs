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
                        LoginDetailList = LoginDetailList.Where(x=>x.LoginTime >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.LoginTime <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo).AddDays(1).AddTicks(-1) : DateTime.Now)).ToList();
                        return Json(new { data = LoginDetailList }, JsonRequestBehavior.AllowGet);                       
                    case "2":                        
                        var BKRY_ORDERList = db.BKRY_DELIVERY.Join(db.BKRY_ORDER,a=>a.OrderId,b=>b.Id, (a, b) => new { a, b }).Select(s => new BKRY_ORDER2
                        {
                            OrderId = ((int)s.a.OrderId).ToString(),
                            personname = s.b.personname,
                            address = s.b.address,
                            postCode = s.b.postCode,
                            add_dtee = ((DateTime)s.b.inserted_dt).ToString(),
                            Orderon = (DateTime)s.b.inserted_dt,
                            Delivered = s.a.DeliveryStatus == 0 ? "No" : s.a.DeliveryStatus == 1 ? "Dispatched" : s.a.DeliveryStatus == 2 ? "Near By" : "Yes",
                            PaymentType = s.a.PaymentType == 0 ? "Cash on Delivery" : "Credit Card Payment"
                        }).ToList().Where(y => y.Orderon >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && y.Orderon <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo).AddDays(1).AddTicks(-1) : DateTime.Now)) ;
                        
                        return Json(new { data = BKRY_ORDERList.ToList<BKRY_ORDER2>() }, JsonRequestBehavior.AllowGet);
                        
                    case "3":
                        List<BKRY_ITEMS> bkryList = db.BKRY_ITEMS.ToList<BKRY_ITEMS>();
                        bkryList = bkryList.Where(x => x.add_date >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.add_date <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo).AddDays(1).AddTicks(-1) : DateTime.Now)).ToList<BKRY_ITEMS>();
                        foreach (BKRY_ITEMS item in bkryList)
                        {
                            item.image = null;
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