using System;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Areas.HelpPage.ModelDescriptions;
using WebApi.Areas.HelpPage.Models;

namespace WebApi.Areas.HelpPage.Controllers
{
    public class WebController : Controller
    {
        private const string ErrorViewName = "Error";

        public WebController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public WebController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            //var displayOrder = new System.Collections.Generic.Dictionary<Type, int>();
            //displayOrder.Add(typeof(CustomerController), 1);
            //displayOrder.Add(typeof(DriverController), 2);
            //ViewBag.DisplayOrder = displayOrder;

            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
	}
}