using API.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WhiteCoat.API.Controllers
{
    public abstract class BaseController : ApiController
    {
        public APIUserInfo CurrentUser
        {
            get
            {
                return new APIUserInfo(this.User as ClaimsPrincipal);
            }
        }
    }
}
