using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;   
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using log4net;

namespace App.WebAPI.Filters
{
    public class LogActionFilter : Attribute, IActionFilter

    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var requestTime = DateTime.Now;
            var requestUri = context.HttpContext.Request.GetDisplayUrl();
            var requestUserName = "";
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                requestUserName = context.HttpContext.User.Identity.Name;
            }

            var requestBody = context.HttpContext.Request.Body.ToString();
            var requestHeaders = context.HttpContext.Request.Headers.ToString();
            var requestQueryString = context.HttpContext.Request.QueryString.ToString();
            var requestHttpVerb = context.HttpContext.Request.Method;

            //var requestBody = "";
            //var req = context.HttpContext.Request;
            //req.EnableRewind();

            //using (var reader
            //    = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            //{
            //    requestBody = reader.ReadToEnd();
            //}

            //req.Body.Position = 0;

            LogicalThreadContext.Properties["RequestTime"] = requestTime;
            LogicalThreadContext.Properties["RequestURI"] = requestUri;
            LogicalThreadContext.Properties["RequestUserName"] = requestUserName;
            LogicalThreadContext.Properties["RequestHeaders"] = requestHeaders;
            LogicalThreadContext.Properties["RequestBody"] = requestBody;
            LogicalThreadContext.Properties["RequestQueryString"] = requestQueryString;
            LogicalThreadContext.Properties["RequestHttpVerb"] = requestHttpVerb;

            Log.Info("----REQUEST-------------------------");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var responseTime = DateTime.Now;
            var requestUriR = context.HttpContext.Request.GetDisplayUrl();
            var responseUserName = "";
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                responseUserName = context.HttpContext.User.Identity.Name;
            }

            var responseHeaders = context.HttpContext.Response.Headers.ToString();
            var responseStatusCode = context.HttpContext.Response.StatusCode.ToString();

            LogicalThreadContext.Properties["ResponseTime"] = responseTime;
            LogicalThreadContext.Properties["RequestURI_r"] = requestUriR;
            LogicalThreadContext.Properties["ResponseUserName"] = responseUserName;
            LogicalThreadContext.Properties["ResponseHeaders"] = responseHeaders;
            LogicalThreadContext.Properties["ResponseStatusCode"] = responseStatusCode;

            Log.Info("----RESPONSE-------------------------");
        }
    }

}