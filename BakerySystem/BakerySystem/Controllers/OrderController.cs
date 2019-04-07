using BakerySystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Getorder(int id)
        {
            List<BKRY_DELIVERY> bkryList = new List<BKRY_DELIVERY>();
            List<BKRY_ITEMS> lstBKRY_ITEMS = new List<BKRY_ITEMS>();

            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.BKRY_DELIVERY.Where(x => x.Id == id).ToList<BKRY_DELIVERY>();
                foreach (BKRY_DELIVERY item in bkryList)
                {
                    lstBKRY_ITEMS.Add(JsonConvert.DeserializeObject<BKRY_ITEMS>(item.OrderDetails));
                }
                //}
                //var commondata = from x in bkryList join lstBKRY_ITEMS 
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        }
    
}