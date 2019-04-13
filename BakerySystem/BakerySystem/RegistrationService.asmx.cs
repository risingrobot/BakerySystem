using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using BakerySystem.Models;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace BakerySystem
{
    /// <summary>
    /// Summary description for RegistrationService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class RegistrationService : System.Web.Services.WebService
    {

        [WebMethod]
        public void UserNameExists(string userName)
        {
            bool userNameInUse = false; 
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {

                var username = new SqlParameter("@UserName", userName);
                var objuserNameInUse =db.Database.SqlQuery<Registration>("spUserNameExists @UserName", username);

            }

            Registration registration = new Registration();
            registration.Username = userName;
            registration.Usernameinuse = userNameInUse;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(registration));
                
        }
    }
}
