using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakerySystem.Models;

namespace BakerySystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Autherize(BakerySystem.Models.SYS_USR_INFO userModel)
        {
            if ("admin" == userModel.UserName && "admin" == userModel.UserPassowrd)
            {
                Session["userID"] = "1";
                Session["userName"] = "admin";
                return View("~/Views/adminPanel/Index.cshtml");
            }
            else
            {
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    var userDetails = db.SYS_USR_INFO.Where(x => x.UserName == userModel.UserName && x.UserPassowrd == userModel.UserPassowrd).FirstOrDefault();
                    if (userDetails == null)
                    {
                        userModel.LoginErrorMessage = "Wrong username or password.";
                        return View("Index", userModel);
                    }
                    else
                    {
                        Session["userID"] = userDetails.UserId;
                        Session["userName"] = userDetails.UserName;
                        LoginDetail ld = new LoginDetail();
                        ld.UserId = userDetails.UserId;
                        ld.UserName = userDetails.UserName;
                        ld.OperatingSystem = userDetails.OperatingSystem;
                        ld.MachineIP = userDetails.MachineIP;
                        ld.UserPassowrd = userDetails.UserPassowrd;
                        ld.LoginTime = DateTime.Now;
                        db.LoginDetails.Add(ld);
                        db.SaveChanges();                        
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }

        public ActionResult LogOut()
        {
            int userId = Session != null && Session["userID"] != null && Session["userID"].ToString() != "" ? Convert.ToInt16(Session["userID"]) : 0;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}

