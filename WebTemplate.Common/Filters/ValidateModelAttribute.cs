using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Common.Models;

namespace WebTemplate.Common.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ValidateModelAttribute> _logger;
        public ValidateModelAttribute(ILogger<ValidateModelAttribute> logger)
        {
            _logger = logger;
        } 
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Response.StatusCode == 403)
                return;
            if (context.HttpContext.Response.StatusCode == 401)
                return;

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                if (errors.Count > 0)
                {
                    string ErrorMessage = string.Empty;
                    foreach (var v in errors)
                    {
                        ModelError me = v.First<ModelError>();
                        if (me.Exception != null)
                            ErrorMessage += me.Exception.Message + "|" + me.ErrorMessage + "|";
                        else
                            ErrorMessage += me.ErrorMessage + "|";
                    }
                    ErrorMessage = ErrorMessage.TrimEnd('|');
                    ResultModel rm = new ResultModel();
                    rm.Code = "FAIL";
                    rm.Message = ErrorMessage;
                    rm.HasError = true;
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new JsonResult(rm);
                }
            } 
        }
    }
}
