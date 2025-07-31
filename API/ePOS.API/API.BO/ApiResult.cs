using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.BO
{
    public class ApiResult
    {
        ///<summary>
        /// Error Code
        ///</summary>
        public ErrorCodes errorCode { get; set; }
        /// <summary>
        /// Result message
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// Result data
        /// </summary>
        public object data { get; set; }
        ///// <summary>
        ///// Permission
        ///// </summary>
        //public object permission { get; set; }

        /// <summary>
        /// API result
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="resultMessage"></param>
        /// <param name="resultData"></param>
        public ApiResult(ErrorCodes errorCode, string resultMessage, object resultData = null )
        {
            this.errorCode = errorCode;
            this.message = resultMessage;
            data = resultData; 
        }
        
    }
}