using BakerySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class ShowItemsController : Controller
    {
        public ActionResult AddtoCart(int id)
        {
            if (Session == null || Session["Cart"] == null)
            {
                List<int> catlist = new List<int>
                {
                    id
                };
                Session["Cart"] = catlist;
            }
            else
            {
                ((List<int>)Session["Cart"]).Add(id);
            }
            return Content(Session["Cart"] != null ? ((List<int>)Session["Cart"]).Count.ToString() : "0");
        }
        public ActionResult RemoveItemCart(int id)
        {
            if (Session != null && Session["Cart"] != null)
            {
                ((List<int>)Session["Cart"]).Remove(id);
            }
            return Content(Session["Cart"] != null ? ((List<int>)Session["Cart"]).Count.ToString() : "0");
        }
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetCartData()
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
                    return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);
                }

            }            
            return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowCategoryForm()
        {
            if (Session != null && Session["Cart"] != null && ((List<int>)Session["Cart"]).Count > 0)
            {
                return PartialView("~/Views/Items/ShoppingCart.cshtml");
            }
            return RedirectToAction("Index","home");
        }

    }
}