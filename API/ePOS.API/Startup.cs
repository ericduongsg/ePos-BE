using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using WhiteCoat.API.Handlers;

[assembly: OwinStartup(typeof(WhiteCoat.API.Startup))]
namespace WhiteCoat.API
{
    public class Startup
    {
        //private string API_URL = Core_App.BL.Utilities.getMainAppURL();
        //System.Configuration.ConfigurationManager.AppSettings["API_URL"].ToString();

        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
 

            HttpConfiguration config = new HttpConfiguration();
            config.Filters.Add(new AuthenticationFilter());
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            

            
        }

        
    }
}