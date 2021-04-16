using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Common.Interfaces;

namespace WebTemplate.Common.Models
{
    public class BaseDto : IResultModel
    {
        public BaseDto()
        {
            this.HasAlert = false;
            this.HasError = false;
            this.Success = true;
            this.Message = string.Empty;
        }
        public bool Success { get; set; }

        public bool HasAlert { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }

        public string SystemMessage { get; set; }

        public string Code { get; set; }
        public object InputValue { get; set; }
        public object OutPutValue { get; set; }


        public void AlertMessage(string mes)
        {

            this.Message = mes;
            this.Success = false;
            this.HasAlert = true;
        }

        public void ErrorMessage(string mes)
        {
            this.Message = mes;
            this.Success = false;
            this.HasError = true;

        }
        public void SuccessSet()
        {

        }
    }
}
