using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTemplate.Common.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }
        public bool HasError { get; set; }
        public bool HasAlert { get; set; }        
        public string Message { get; set; }

        public string Code { get; set; } //SUCCESS,FAIL,ERROR,ALERT

        public object InputValue { get; set; }
        public object OutPutValue { get; set; }

    }
}
