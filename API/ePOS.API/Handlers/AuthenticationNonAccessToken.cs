using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using API.BL;
using System.Configuration;
using API.BO;

namespace Handlers
{
    public class AuthenticationNonAccessToken : ActionFilterAttribute
    {
        public AccountType account_type = AccountType.Developer;
        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            string basic_authorization = string.Empty;
            switch (account_type)
            {
                case AccountType.Developer:// --> using for Test api
                    basic_authorization = "Basic YXBpRGV2ZWxvcGVyOm5BJG5jbVljVDd5";	// username: apiDeveloper, password: nA$ncmYcT7y
                    break;
                case AccountType.Member:
                    basic_authorization = "Basic YXBpTWVtYmVyOmI/V0Nqck45bmc0"; // username: apiMember, password: b?WCjrN9ng4
                    break;
                case AccountType.Doctor:
                    basic_authorization = "Basic YXBpRG9jdG9yOk4hRER3OURXQzBj"; // username: apiDoctor, password: N!DDw9DWC0c
                    break;
                case AccountType.Driver:
                    basic_authorization = "Basic YXBpRHJpdmVyOkEhRFJpNURWRTRy"; // username: apiDriver, password: A!DRi5DVE4r
                    break;
                case AccountType.Pharmacy:
                    basic_authorization = "Basic YXBpUGhhcm1hY3k6QSFQSGE4Uk1DNFk="; // username: apiPharmacy, password: A!PHa8RMC4Y
                    break;
                case AccountType.Web:
                    basic_authorization = "Basic YXBpV2ViOkQhVE1EMTdOQnQ5WQ=="; // username: apiWeb, password: D!TMD17NBt9Y
                    break;

            }
            
             if (httpActionContext.Request.Headers.Authorization != null && httpActionContext.Request.Headers.Authorization.ToString() == basic_authorization)
            {
                base.OnActionExecuting(httpActionContext);
            }
             //else if (httpActionContext.Request.Headers.Authorization.Scheme.ToLower() == "bearer")
             //{
             //    base.OnActionExecuting(httpActionContext);
             //}
            else
            {
                throw new HttpResponseException(httpActionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorString.UNAUTHORIZED));
            }
        }
    }
}