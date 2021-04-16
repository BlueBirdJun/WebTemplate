using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Common.Models;

namespace WebTemplate.Common.Filters
{
    public class ExceptionLogFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionLogFilter> _logger;
        private readonly ILifetimeScope _scope;
        private readonly IConfiguration _con;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ExceptionLogFilter(ILogger<ExceptionLogFilter> logger, ILifetimeScope scope, IConfiguration configuration)
        {
            _con = configuration;
            _logger = logger;
            this._scope = scope;            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception; 
            ResultModel rm = new ResultModel()
            {
                //Success = false,
                HasError = true,
                Message = $"{(exception.InnerException == null ? "" : exception.InnerException.Message)}/{exception.Message}/{exception.StackTrace}"
            };
            string groupid = string.Empty;            
            string msg = $"Path:{context.HttpContext.Request.Path}" +
                $"({context.HttpContext.Request.Method.ToString()})\r\n" +                
                $"query:{context.HttpContext.Request.QueryString.Value}\r\n" +
                $"Message:{rm.Message}\r\n";
            rm.Code = "ERROR";
            if (exception.Message.Equals("Object reference not set to an instance of an object."))
                rm.Message = "Api Parameter Error";//exception.Message;
            else
                rm.Message = "Server Error";//exception.Message;             
            _logger.LogError(msg);
            context.HttpContext.Response.StatusCode = 500;
            context.Result = new JsonResult(rm);
            context.ExceptionHandled = true;
        }

         

    }
     
}
