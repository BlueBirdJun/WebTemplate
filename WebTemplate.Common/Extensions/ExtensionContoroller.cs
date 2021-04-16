using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTemplate.Common.Extensions
{
    public class ExtensionContoroller: ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Ok1()
        {
            return Ok();
        }
    }
}
