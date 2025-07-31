using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Handlers
{
    public class ValidateInputModelAttribute : ActionFilterAttribute
    {
        private readonly List<string> _actionNoParameters = new List<string> { "getmasterdata", "getmedication", "gettwiliotoken", "searchrecommendeddoctor", "getactivebooking", "", "getcardlist", "getactiverefillmedication", "checkrefillmedication", "getrefillmedication", "editprofilerequest", "getprofile", "gettwiliotoken", "getincomingpatient", "getconsultingpatient", "getscanqrref", "", "pushnotificationincms", "pushnotification" };

        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            string actionName = httpActionContext.ControllerContext.RouteData.Values["action"].ToString().ToLower();

            if (
                    !_actionNoParameters.Contains(actionName) &&
                    httpActionContext.ActionArguments.Count == 0

               )
            {
                throw new HttpResponseException(httpActionContext.Request.CreateResponse(HttpStatusCode.BadRequest, API.BO.ErrorString.BAD_REQUEST));
            }
            else if (
                        !_actionNoParameters.Contains(actionName) &&
                        httpActionContext.ActionArguments["model"] == null

                    )
            {
                throw new HttpResponseException(httpActionContext.Request.CreateResponse(HttpStatusCode.BadRequest, API.BO.ErrorString.BAD_REQUEST));
            }
            else
            {
                if (httpActionContext.ModelState.IsValid)
                {
                    base.OnActionExecuting(httpActionContext);
                }
                else
                {
                    string invalid = API.BO.ErrorString.OK;
                    for (int i = 0; i < httpActionContext.ModelState.Count; i++)
                    {
                        foreach (var error in httpActionContext.ModelState.ElementAt(i).Value.Errors)
                        {
                            invalid = error.ErrorMessage;
                            if (invalid == "")
                            {
                                invalid = error.Exception.Message.ToString();
                            }
                            i = httpActionContext.ModelState.Count;
                            break;

                        }
                    }
                    if (httpActionContext.ModelState.Count > 0)
                    {
                        throw new HttpResponseException(httpActionContext.Request.CreateResponse(HttpStatusCode.OK, invalid));
                    }
                    else
                    {
                        throw new HttpResponseException(httpActionContext.Request.CreateResponse(HttpStatusCode.BadRequest, invalid));
                    }

                    //List<string> errors = new List<string>();

                    //foreach (var state in httpActionContext.ModelState)
                    //{
                    //    foreach (var error in state.Value.Errors)
                    //    {
                    //        errors.Add(error.ErrorMessage);
                    //    }
                    //}
                    //throw new HttpResponseException(httpActionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors));

                }
            }
        }
    }
}