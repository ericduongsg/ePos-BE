using System.Web.Http;
using System.Web.Mvc;

namespace WebApi.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            
            context.MapRoute(
                "HelpPage_Member",
                "doc/Member/{action}/{apiId}",
                new { controller = "Member", action = "Index", apiId = UrlParameter.Optional });

            context.MapRoute(
                "HelpPage_Doctor",
                "doc/Doctor/{action}/{apiId}",
                new { controller = "Doctor", action = "Index", apiId = UrlParameter.Optional });

            context.MapRoute(
                "HelpPage_Driver",
                "doc/Driver/{action}/{apiId}",
                new { controller = "Driver", action = "Index", apiId = UrlParameter.Optional });

            context.MapRoute(
                "HelpPage_Pharmacy",
                "doc/Pharmacy/{action}/{apiId}",
                new { controller = "Pharmacy", action = "Index", apiId = UrlParameter.Optional });

            context.MapRoute(
               "HelpPage_Web",
               "doc/Web/{action}/{apiId}",
               new { controller = "Web", action = "Index", apiId = UrlParameter.Optional });


            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
