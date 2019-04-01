using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySystem.Models;
using System.Data.Entity;

namespace BakerySystem.Controllers
{
    public class ItemsController : Controller
    {
        // GET: Items
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                List<BKRY_ITEMS> bkryList = db.BKRY_ITEMS.ToList<BKRY_ITEMS>();
                return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new BKRY_ITEMS());
            else
            {
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    return View(db.BKRY_ITEMS.Where(x => x.Id == id).FirstOrDefault<BKRY_ITEMS>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(BKRY_ITEMS bkt)
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                if (bkt.Id == 0)
                {
                    db.BKRY_ITEMS.Add(bkt);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(bkt).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                BKRY_ITEMS emp = db.BKRY_ITEMS.Where(x => x.Id == id).FirstOrDefault<BKRY_ITEMS>();
                db.BKRY_ITEMS.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}