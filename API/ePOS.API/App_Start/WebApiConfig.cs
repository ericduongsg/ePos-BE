using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Core_App.BL;
using Handlers;

namespace WhiteCoat.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                                         Constants.PROJECT_NAME + "Api",	// Route name
                                         routeTemplate: string.Format("{0}/{{controller}}/{{action}}", Constants.VERSION),
                                         defaults: null,
                                         constraints: null
                                         , handler: HttpClientFactory.CreatePipeline(
                                         new HttpControllerDispatcher(config),
                                         new DelegatingHandler[] {
                                                new WrappingHandler()
                                        }));

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}
