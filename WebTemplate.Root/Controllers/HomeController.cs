using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTemplate.Root.Controllers
{
    public class HomeController : ControllerBase
    {
        
        public IActionResult Index()
        {
            return Ok();
            //return new RedirectResult("http://naver.com");
            //return Ok("aaa");
        }
    }
}
