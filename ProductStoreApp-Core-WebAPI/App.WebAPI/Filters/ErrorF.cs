using log4net; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.WebAPI.Filters
{
    public class LogErrorFilter : Attribute, IExceptionFilter
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void OnException(ExceptionContext context)
        {
            var exceptionMessage = context.Exception.Message;
            var stackTrace = context.Exception.StackTrace;

            LogicalThreadContext.Properties["ExceptionMessage"] = exceptionMessage;
            LogicalThreadContext.Properties["StackTrace"] = stackTrace;

            Log.Info("----ERROR-------------------------");
        }
        //public void OnException(ExceptionContext filterContext)
        //{ 
                //Log_Event log = new Log_Event
                //{
                //    RequestTime = DateTime.Now,
                //    RequestURI = Convert.ToString(filterContext.RequestContext.HttpContext.Request.Url),
                //    Username = "",
                //    Headers = "",
                //    Body = "",
                //    QueryString = "",
                //    HTTPMethod = filterContext.RequestContext.HttpContext.Request.HttpMethod,
                //    ErrorStackTrace = filterContext.Exception.StackTrace,
                //    ErrorMessage = filterContext.Exception.Message
                //};
                //if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
                //{
                //    log.Username = filterContext.RequestContext.HttpContext.User.Identity.Name;
                //}

                //var headers = filterContext.RequestContext.HttpContext.Request.Headers.AllKeys;
                //foreach (var header in headers)
                //{
                //    log.Headers += header + "=" + filterContext.RequestContext.HttpContext.Request.Headers[header] + ";";
                //} 
                //var queryString = filterContext.RequestContext.HttpContext.Request.QueryString.AllKeys;
                //foreach (var query in queryString)
                //{
                //    log.QueryString += query + "=" + filterContext.RequestContext.HttpContext.Request.QueryString[query] + ";";
                //}
                 
                //log4net.LogicalThreadContext.Properties["req_time"] = DateTime.Now;
                //log4net.LogicalThreadContext.Properties["req_uri"] = log.RequestURI;
                //log4net.LogicalThreadContext.Properties["username"] = log.Username;
                //log4net.LogicalThreadContext.Properties["headers"] = log.Headers;
                //log4net.LogicalThreadContext.Properties["body"] = log.Body;
                //log4net.LogicalThreadContext.Properties["query_string"] = log.QueryString;
                //log4net.LogicalThreadContext.Properties["http_method"] = log.HTTPMethod;
                //log4net.LogicalThreadContext.Properties["error_stack_trace"] = log.ErrorStackTrace;
                //log4net.LogicalThreadContext.Properties["error_message"] = log.ErrorMessage;
                //Log4net.Log.Error("LOG_ERROR");

            }
         

    }

//}