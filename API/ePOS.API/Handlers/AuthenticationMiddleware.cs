using API.BO;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WhiteCoat.API.Handlers
{
    public class AuthenticationMiddleware : OwinMiddleware
    {
        public const string OwinChallengeFlag = "X-Challenge";
        public AuthenticationMiddleware(OwinMiddleware next) : base(next) { }

         
        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);
            if (context.Response.StatusCode == 400 && context.Response.Headers.ContainsKey(OwinChallengeFlag))
            {
                ApiResult result = new ApiResult(ErrorCodes.ACCOUNT_NOT_FOUND, ErrorString.ACCOUNT_NOT_FOUND, null);
 
                var headerValues = context.Response.Headers.GetValues(OwinChallengeFlag);
                context.Response.Headers.Remove(OwinChallengeFlag);

            

                //await context.Response.WriteAsync("Hello, world.");
            }
            
        }
    }
}