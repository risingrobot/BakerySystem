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
        public ActionResult Index(int id=0)
        {
            SYS_USR_INFO obj = new SYS_USR_INFO();
            if (id != 0)
            {
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    obj = db.SYS_USR_INFO.Where(x=>x.UserId == id).FirstOrDefault();
                }
                return View(obj);
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(SYS_USR_INFO objOn)
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
            }
            SYS_USR_INFO userinfo = new SYS_USR_INFO();

                try
                {
                    using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                    {
                    SYS_USR_INFO obj = new SYS_USR_INFO
                    {
                        UserId = (objOn.UserId > 0) ? objOn.UserId : 0,
                        UserName = objOn.UserName,
                        UserPassowrd = objOn.UserPassowrd,
                        MachineIP = ipAddress,
                        OperatingSystem = osVersion,
                        LoginTime = DateTime.Now,
                        LoginErrorMessage = JsonConvert.SerializeObject(userinfo),
                        address = objOn.address,
                        email = objOn.email,
                        street = objOn.street,
                        phone = objOn.phone,
                        postCode = objOn.postCode                        
                    };
                    
                    if (db.SYS_USR_INFO.Where(x => x.UserName == obj.UserName).ToList().Count == 0)
                    {
                        db.SYS_USR_INFO.Add(obj);
                        db.Entry(obj).State = EntityState.Added;

                        db.SaveChanges(); // fuck you exception fuck you Bitch
                        Session["Message"] = "Saved Successfully your UserName# is " + obj.UserName.ToString();                        
                    }
                    else
                    {
                        db.SYS_USR_INFO.Remove(db.SYS_USR_INFO.Where(x=>x.UserName == obj.UserName).ToList().FirstOrDefault());
                        db.SYS_USR_INFO.Add(obj);
                        db.SaveChanges(); // fuck you exception fuck you Bitch
                        Session["Message"] = "Updated Successfully your UserName# is " + obj.UserName.ToString();                        
                    }
                    return RedirectToAction("LogOut", "Login");
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
        public JsonResult DeleteUser(string id)
        {
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {

                var search = db.SYS_USR_INFO.ToList().Where(x => x.UserId == Convert.ToInt32(id)).FirstOrDefault();
                db.SYS_USR_INFO.Remove(search);
                db.SaveChanges();                
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowProfile()
        {           
            return View();
        }
        [HttpGet]
        public ActionResult GetData()
        {
            List<SYS_USR_INFO> ListSYS_USR_INFO = new List<SYS_USR_INFO>();
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                ListSYS_USR_INFO = db.SYS_USR_INFO.ToList();
            }
            return Json(new { data = ListSYS_USR_INFO }, JsonRequestBehavior.AllowGet);
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