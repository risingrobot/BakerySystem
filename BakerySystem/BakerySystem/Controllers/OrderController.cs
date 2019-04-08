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
            List<GetOrderDetail> bkryList = new List<GetOrderDetail>();
            List<BKRY_ITEMS> lstBKRY_ITEMS = new List<BKRY_ITEMS>();

            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.GetOrderDetails.Where(x => x.OrderId == id).ToList<GetOrderDetail>();
                foreach (var item in bkryList)
                {
                    item.OrderDetails = null;
                    item.address = item.address + " " + item.street + " " + item.postCode; 
                }
                return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetorderDetails(int id)
        {
            List<GetOrderDetail> bkryList = new List<GetOrderDetail>();
            List<BKRY_ITEMS> lstBKRY_ITEMS = new List<BKRY_ITEMS>();

            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.GetOrderDetails.Where(x => x.OrderId == id).ToList<GetOrderDetail>();
                foreach (var item in bkryList)
                {
                    lstBKRY_ITEMS= Newtonsoft.Json.JsonConvert.DeserializeObject<List<BKRY_ITEMS>>(item.OrderDetails);
                    item.address = item.address + " " + item.street + " " + item.postCode;
                }
                return PartialView("~/Views/Order/OrderDetails.cshtml", lstBKRY_ITEMS);
            }
        }

    }
    //class orderObject
    //{
    //    public Nullable<int> OrderId { get; set; }  
       
    //    public Nullable<System.DateTime> Orderon { get; set; }
    //    public string personname { get; set; }       
    //    public string address { get; set; }
    //    public string street { get; set; }
    //    public string postCode { get; set; }
    //    public string Delivered { get; set; }        
    //    public string Itemname { get; set; }
    //    public Nullable<decimal> Itemprice { get; set; }        
    //    public string expiry_dte { get; set; } 
    //    public Nullable<long> categoryId { get; set; }
    //    public Nullable<int> quantity { get; set; }
    //}
    
}