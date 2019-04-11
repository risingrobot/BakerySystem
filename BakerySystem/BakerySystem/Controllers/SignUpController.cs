using BakerySystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
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

            if (userAgentText != null)
            {
                int startPoint = userAgentText.IndexOf('(') + 1;
                int endPoint = userAgentText.IndexOf(';');

                string osVersion = userAgentText.Substring(startPoint, (endPoint - startPoint));
                //string friendlyOsName = osList[osVersion];

                SYS_USR_INFO info = new SYS_USR_INFO() { MachineIP = ipAddress, OperatingSystem = osVersion };
                return View("~/Views/SignUp/Index.cshtml", info);
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(SYS_USR_INFO objOn)
        {
            SYS_USR_INFO userinfo = new SYS_USR_INFO();



                try
                {
                    using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                    {
                        SYS_USR_INFO obj = new SYS_USR_INFO();

                       
                    //  obj.OrderDetails = JsonConvert.SerializeObject(bkryList);
                    obj.UserName = objOn.UserName;
                    obj.UserPassowrd = objOn.UserPassowrd;
                    obj.MachineIP = objOn.MachineIP;
                    obj.OperatingSystem = objOn.OperatingSystem;
                    obj.LoginTime = DateTime.Now;
                    obj.LoginErrorMessage = JsonConvert.SerializeObject(userinfo);
                    db.SYS_USR_INFO.Add(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges(); // fuck you exception fuck you Bitch
                    Session["Message"] = "Saved Successfully your UserName# is " + obj.UserName.ToString();
                    return RedirectToAction("Index", "home");
                    }
                }
                catch (Exception ex) { return Json(new { success = true, message = "UnSuccessfully" + ex.ToString() }, JsonRequestBehavior.AllowGet); }
            
        }

        [AllowAnonymous]
        public JsonResult UserAlreadyExistsAsync(string user)
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {

                var search = db.SYS_USR_INFO.Where(x => x.UserName == user).SingleOrDefault();
                if (search != null)
                    return Json(search, JsonRequestBehavior.AllowGet);
                else return Json(0);
            }
        }


        //[AllowAnonymous]
        //public async Task<JsonResult> UserAlreadyExistsAsyncOne(string username)
        //{
        //    var result =
        //        await FindByUserName(username);
               
        //    return Json(result == null, JsonRequestBehavior.AllowGet);
        //}

        //static async Task<JsonResult> FindByUserName(string username)
        //{
        //    using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
        //    {
        //        var search = db.SYS_USR_INFO.Where(x => x.UserName == username).SingleOrDefault();
        //        if (search != null)
        //            return JsonResult(search);



        //    }
        //}

    }
}