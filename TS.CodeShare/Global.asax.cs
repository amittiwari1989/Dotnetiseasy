using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TS.CodeShare.Helpers;

namespace TS.CodeShare
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(); 
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }

        void Application_Error(object sender, EventArgs e)
        {
            string ip = CommonStuff.GetLocalIPAddress(); // Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            Exception objErr = Server.GetLastError().GetBaseException();

            string objErrMgs = objErr.StackTrace;

            string err = "Error Date : " + DateTime.Now.ToString() + " , Error in: " + Request.Url.ToString() + " , Error Message:" + objErr.Message.ToString() + " ipaddress : " + ip;
            

            string path = HttpContext.Current.Server.MapPath("~/Logs/ErrorLog_GlobalErrors.txt");

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, true))
            {
                sw.WriteLine(err + objErrMgs);
                sw.WriteLine(objErr.StackTrace);
                sw.Close();
            }
            
            CommonStuff.SendEmail("tiwariamitkumar70@gmail.com", "Error in DotNetIsEasy.com", objErr.Message.ToString() + "<br />IP: " + ip + "<br /><br />" +  objErr.StackTrace);
            Server.ClearError();

            Response.Write(objErr.Message.ToString());

        }

        //void Application_Error(object sender, EventArgs e)
        //{
        //    string ip = CommonStuff.GetLocalIPAddress(); // Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //    Exception objErr = Server.GetLastError().GetBaseException();

        //    string objErrMgs = objErr.StackTrace;

        //    string err = "Error Date : " + DateTime.Now.ToString() + " , Error in: " + Request.Url.ToString() + " , Error Message:" + objErr.Message.ToString() + " ipaddress : " + ip;

        //    try
        //    {
        //        if (Session["uid"] != null)
        //            err += " uid : " + Session["uid"];
        //    }
        //    catch { }

        //    Logger.log.Error(err);
        //    Logger.log.Error(objErr.StackTrace);

        //    Server.ClearError();

        //    Response.Write(objErr.Message.ToString());

        //}
    }
}
