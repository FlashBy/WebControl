using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Timers;
using WebControl.OPC;

namespace WebControl
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Session_Start(object sender, EventArgs e)
        {
            string sessionId = Session.SessionID;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(saveDB);

          //  System.Timers.Timer validateTimer = new System.Timers.Timer(1000 * 60 * 60 * 24);
            System.Timers.Timer validateTimer = new System.Timers.Timer(10000);
            validateTimer.AutoReset = true;
            validateTimer.Enabled = true;
            validateTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.validate); 
        }
        public void saveDB(Object sender, ElapsedEventArgs e)
        {
            Timer timer = sender as Timer;
            if (timer == null)
                return;
            WebControl.OPC.OPCControl.SaveToDB();
        }
        public void validate(Object sender, ElapsedEventArgs e)
        {
            Timer timer = sender as Timer;
            if (timer == null)
                return;
            WebControl.OPC.OPCControl.vaildateDB();
        }
    }
   
}