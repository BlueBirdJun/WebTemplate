using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Common.Helpers;
using WebTemplate.Common.Models;

namespace WebTemplate.Common.Filters
{
    public class KeyAuthorFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sss = CrytoHelper.Encrypt(DateTime.Now.ToString("dd:yyyyMM TENBYTENCUBE"));
            //return;
            ResultModel rm = new ResultModel();
            if (context.HttpContext.Request.Headers["seqkey"].Count() == 0)
            {
                rm.Code = "No Auth";
                rm.Message = "권한없음";
                context.HttpContext.Response.StatusCode = 403;// HttpStatusCode.H403.ToString().ToInt();
                context.Result = new JsonResult(rm);
                return;
            }

            ///키는 yyyy-MM-dd HH:mm  ddmm:HHyyyyMM
            string key = context.HttpContext.Request.Headers["seqkey"][0].Trim();
            //key = key.Base64Encode();
            try
            {
                key = CrytoHelper.Decrypt(key);
            }
            catch (Exception exc)
            {
                rm.Code = "No Auth";
                rm.Message = "KEY값 틀림";
                context.HttpContext.Response.StatusCode = 403;// HttpStatusCode.H403.ToString().ToInt();
                context.Result = new JsonResult(rm);
                return;
            }
            //매칭 키

            if (!key.Equals(DateTime.Now.ToString("dd:yyyyMM TENBYTENCUBE")))
            {
                rm.Code = "No Auth";
                rm.Message = "KEY값 틀림";
                context.HttpContext.Response.StatusCode = 403;// HttpStatusCode.H403.ToString().ToInt();
                context.Result = new JsonResult(rm);
                return;
            }
        }
    }
}
