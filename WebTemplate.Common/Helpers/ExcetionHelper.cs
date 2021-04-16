using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTemplate.Common.Helpers
{
    public class ExcetionHelper
    {
        public static string ExceptionMessage(Exception exc)
        {
            string rt = string.Empty;
            var st = new System.Diagnostics.StackTrace(exc, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            var l1 = frame.GetFileName();
            var l2 = frame.GetMethod();
            var l3 = frame.GetFileColumnNumber();
            rt = $"{exc.Message}";
            if (exc.InnerException != null)
                rt += $"|{exc.InnerException.Message}";
            rt += $"|{l1}|{l2}|{line.ToString()}";
            return rt;
        }
    }
}
