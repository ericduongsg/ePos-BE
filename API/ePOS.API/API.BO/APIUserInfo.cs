using Core_App.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace API.BO
{
    public class APIUserInfo: ClaimsPrincipal
    {
        public APIUserInfo(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string UserID
        {
            get
            {
                return this.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }
        public string DecryptUserID
        {
            get
            {
                return Utilities.decryptRijndael(this.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Constants.RijndaelHashType.PRIVATE);
            }
        }
        public string Role
        {
            get
            {
                return this.FindFirst(ClaimTypes.Role).Value;
            }
        }
        public string RefreshToken
        {
            get
            {
                return this.FindFirst("rfr-tkn").Value;
            }
        }
        //public string Name
        //{
        //    get
        //    {
        //        return this.FindFirst(ClaimTypes.Name).Value;
        //    }
        //}

        //public string UserName
        //{
        //    get
        //    {
        //        return this.FindFirst("uname").Value;
        //    }
        //}
    }
}