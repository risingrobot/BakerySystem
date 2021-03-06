﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySystem.Models;
using System.Data.Entity;
using System.IO;
using System.Drawing;
using System.Data.Entity.Validation;
using System.Web.UI;

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
                foreach (BKRY_ITEMS item in bkryList)
                {
                    item.image = null;
                    item.add_dtee = Convert.ToDateTime(item.add_date).ToLongDateString();
                }


                return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            List<BKRY_CATEGORY> bkryList = null;
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.BKRY_CATEGORY.ToList<BKRY_CATEGORY>();

            }
            ViewBag.Category = bkryList;
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
        public ActionResult AddOrEdit(BKRY_ITEMS bkt, HttpPostedFileBase file)
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
            bkt.categoryId = Convert.ToInt64(Request.Form["categoryname"] != "" ? Request.Form["categoryname"] : "0");
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                if (bkt.Id == 0)
                {
                    db.BKRY_ITEMS.Add(bkt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                   
                }
                else
                {
                    db.Entry(bkt).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw e;
                    }
                    return RedirectToAction("Index");
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
        public ActionResult ShowItems(int id)
        {
            ViewBag.CID = id;
            return View();
        }
        public ActionResult GetShowItemsData(int id)
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                List<BKRY_ITEMS> bkryList = id == 0 ? db.BKRY_ITEMS.ToList<BKRY_ITEMS>()  : db.BKRY_ITEMS.Where(x => x.categoryId == id).ToList<BKRY_ITEMS>();                
                foreach (BKRY_ITEMS item in bkryList)
                {
                    item.image = null;
                }
                return Json(new { data = bkryList }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}