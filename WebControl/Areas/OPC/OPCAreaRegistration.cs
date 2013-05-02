using System.Web.Mvc;

namespace WebControl.Areas.OPC
{
    public class OPCAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OPC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OPC_default",
                "OPC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
