using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using API.BO;
using System.Net;

namespace Handlers
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResult(request, response);
        }

        private static HttpResponseMessage BuildApiResult(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            object errorMessage = null;
            object apiRespone = null;
            if (response.TryGetContentValue(out content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    HttpError error = content as HttpError;
                    if (error != null)
                    {
                        if (response.StatusCode.ToString() == HttpStatusCode.MethodNotAllowed.ToString())
                        {
                            apiRespone = new ApiResult((ErrorCodes)HttpStatusCode.MethodNotAllowed, ErrorString.METHOD_NOT_ALLOWED, null);
                        }
                        else
                        {
                            errorMessage = error.Message;
                            #if DEBUG
                            errorMessage = string.Concat(errorMessage, error.ExceptionMessage, error.StackTrace);
                            apiRespone = new ApiResult((ErrorCodes)response.StatusCode, errorMessage.ToString(), null);
                            #endif
                        }
                    }
                    else
                    {
                        if (content is ApiResult || content.GetType().Name == typeof(ApiResult).Name)
                        {
                            apiRespone = content;
                        }
                        else
                        {
                            if (content.ToString() == "")
                            {
                                apiRespone = new ApiResult(ErrorCodes.BAD_REQUEST, "Object invalid", null);
                            }
                            else
                            {
                                apiRespone = new ApiResult(ErrorCodes.BAD_REQUEST, content.ToString(), null);
                            }
                            
                        }
                    }
                }
                else
                {
                    if (content is ApiResult || content.GetType().Name == typeof(ApiResult).Name)
                    {
                        apiRespone = content;

                    }
                    else
                    {
                        if(content != API.BO.ErrorString.OK)
                        {
                            apiRespone = new ApiResult(ErrorCodes.BAD_REQUEST, content.ToString(), null);
                            //apiRespone = content;
                        }
                        else
                        {
                            apiRespone = new ApiResult(ErrorCodes.OK, null, content);
                        }
                    }
                }
            }
            else
            {
                apiRespone = new ApiResult(ErrorCodes.BAD_REQUEST, null, null);
            }

            var newResponse = request.CreateResponse(response.StatusCode, apiRespone);

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}