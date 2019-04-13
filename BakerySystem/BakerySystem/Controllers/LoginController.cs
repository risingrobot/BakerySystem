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
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            Dictionary<string, string> osList = new Dictionary<string, string>
        {
            {"Windows NT 6.3", "Windows 8.1"},
            {"Windows NT 6.2", "Windows 8"},
            {"Windows NT 6.1", "Windows 7"},
            {"Windows NT 6.0", "Windows Vista"},
            {"Windows NT 5.2", "Windows Server 2003"},
            {"Windows NT 5.1", "Windows XP"},
            {"Windows NT 5.0", "Windows 2000"}
        };

            string userAgentText = Request.UserAgent;
            string osVersion = "";
            if (userAgentText != null)
            {
                int startPoint = userAgentText.IndexOf('(') + 1;
                int endPoint = userAgentText.IndexOf(';');

                osVersion = userAgentText.Substring(startPoint, (endPoint - startPoint));
                //string friendlyOsName = osList[osVersion];

                SYS_USR_INFO info = new SYS_USR_INFO() { MachineIP = ipAddress, OperatingSystem = osVersion };               
            }
            if ("admin" == userModel.UserName && "admin" == userModel.UserPassowrd)
            {
                Session["userID"] = "1";
                Session["userName"] = "admin";
                Session["ltype"] = "1";
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
                        Session["ltype"] = "2";
                        LoginDetail ld = new LoginDetail();
                        ld.UserId = userDetails.UserId;
                        ld.UserName = userDetails.UserName;
                        ld.OperatingSystem = osVersion;
                        ld.MachineIP = ipAddress;
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

