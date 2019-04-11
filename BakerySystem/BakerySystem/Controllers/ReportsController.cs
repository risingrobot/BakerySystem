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
                        
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                switch (nm_rpttype)
                {
                    case "1":
                        List<LoginDetail> LoginDetailList = db.LoginDetails.ToList<LoginDetail>();
                        LoginDetailList = LoginDetailList.Where(x=>x.LoginTime >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.LoginTime <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo) : DateTime.Now)).ToList();
                        return Json(new { data = LoginDetailList }, JsonRequestBehavior.AllowGet);                       
                    case "2":
                        List<BKRY_ORDER> BKRY_ORDERList = db.BKRY_ORDER.ToList<BKRY_ORDER>();
                        BKRY_ORDERList = BKRY_ORDERList.Where(x => x.inserted_dt >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.inserted_dt <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo) : DateTime.Now)).ToList();
                        return Json(new { data = BKRY_ORDERList }, JsonRequestBehavior.AllowGet);
                       
                    case "3":
                        List<BKRY_DELIVERY> BKRY_DELIVERYList = db.BKRY_DELIVERY.ToList<BKRY_DELIVERY>();
                        BKRY_DELIVERYList = BKRY_DELIVERYList.Where(x => x.inserted_dt >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.inserted_dt <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo) : DateTime.Now)).ToList();
                        return Json(new { data = BKRY_DELIVERYList }, JsonRequestBehavior.AllowGet);                       
                    case "4":
                        List<BKRY_ITEMS> bkryList = db.BKRY_ITEMS.ToList<BKRY_ITEMS>();
                        bkryList = bkryList.Where(x => x.add_date >= (nm_dtfrom != "" ? Convert.ToDateTime(nm_dtfrom) : DateTime.Now) && x.add_date <= (nm_dtTo != "" ? Convert.ToDateTime(nm_dtTo) : DateTime.Now)).ToList();
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