using BakerySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                List<BKRY_CATEGORY> bkryList = db.BKRY_CATEGORY.ToList<BKRY_CATEGORY>();
                foreach (BKRY_CATEGORY item in bkryList)
                {
                    item.image = null;
                }
                return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new BKRY_CATEGORY());
            else
            {
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    return View(db.BKRY_CATEGORY.Where(x => x.Id == id).FirstOrDefault<BKRY_CATEGORY>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(BKRY_CATEGORY bkt, HttpPostedFileBase file)
        {
            try
            {
                byte[] buf = null;
                if (file != null && file.ContentLength > 0)
                {
                    buf = new byte[file.ContentLength];
                    file.InputStream.Read(buf, 0, buf.Length);
                    bkt.image = buf;
                }
                bkt.insert_by = Session != null && Session["UserName"] != null ? Session["UserName"].ToString() : "";
                bkt.add_date = DateTime.Now;
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    if (bkt.Id == 0)
                    {
                        db.BKRY_CATEGORY.Add(bkt);
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
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                BKRY_CATEGORY emp = db.BKRY_CATEGORY.Where(x => x.Id == id).FirstOrDefault<BKRY_CATEGORY>();
                db.BKRY_CATEGORY.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
