using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTemplate.Common.Interfaces
{
    public interface IResultModel
    {
        bool Success { get; set; }
        string Code { get; set; }
        bool HasAlert { get; set; }
        bool HasError { get; set; }
        object InputValue { get; set; }
        string Message { get; set; }
        object OutPutValue { get; set; }
    }
}
