using API.BL;
using API.BO;
using Core_App.BL;
using MyApp.API.Handlers;
using NLog;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http.Filters;

namespace WhiteCoat.API.Handlers
{
    public class AuthenticationFilter : AuthenticationFilterAttribute
    { 
        private Logger logger = LogManager.GetLogger("Write-Log");

        public override void OnAuthentication(HttpAuthenticationContext context)
        {
            System.Net.Http.Formatting.MediaTypeFormatter jsonFormatter =
                new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            var identity = context.Principal.Identity as ClaimsIdentity;

            AuthenticationHeaderValue authHeader = context.Request.Headers.Authorization;
            ApiResult apiResult;

            //// 2. If there are no credentials, do nothing.
            //if (authHeader == null)
            //{
            //    return;
            //}
            string actionName = context.ActionContext.ActionDescriptor.ActionName.ToLower();
            string controllerName = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower();

            if (
                    controllerName == Constants.API_ROLE_NAME.DEV
               )
            {
                return;
            }
            else if (authHeader == null || (authHeader.Scheme != "Bearer" && authHeader.Scheme != "Basic"))
            {
                apiResult = new ApiResult(ErrorCodes.UNAUTHORIZED, ErrorString.UNAUTHORIZED, null);
                context.ErrorResult = new AuthenticationFailureResult("unauthorized", context.Request, apiResult);
            }
            else if (authHeader.Scheme == "Basic")
            {
                string basic_authorization = string.Empty;
                if (controllerName == Constants.API_ROLE_NAME.DEV)
                {
                    if (controllerName == Constants.API_ROLE_NAME.DEV)
                    {
                        basic_authorization = "Basic YXBpRGV2ZWxvcGVyOm5BJG5jbVljVDd5";
                    }
                    
                    if (context.Request.Headers.Authorization.ToString() == basic_authorization)
                    {
                        return;
                    }
                    else
                    {
                        apiResult = new ApiResult(ErrorCodes.UNAUTHORIZED, ErrorString.UNAUTHORIZED, null);
                        context.ErrorResult = new AuthenticationFailureResult("unauthorized", context.Request, apiResult);
                    }
                }
                else
                {
                    apiResult = new ApiResult(ErrorCodes.UNAUTHORIZED, ErrorString.UNAUTHORIZED, null);
                    context.ErrorResult = new AuthenticationFailureResult("unauthorized", context.Request, apiResult);
                }
            }
        }
    }
}