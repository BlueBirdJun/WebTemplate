using Autofac;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Common.Extensions;

namespace Business1.Api.Controllers
{
    [Produces("application/json")]
    [Route("v2/test")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Display(Name = "v1", Description = "v1")]
    public class TestController: ExtensionContoroller
    {
        #region member
        private readonly ILogger<TestController> _logger;
        private readonly ILifetimeScope _scope;
        private readonly IConfiguration _con;
        private readonly IMediator _mediator;
        #endregion

        #region construct
        public TestController(ILogger<TestController> logger, ILifetimeScope scope,IConfiguration con,IMediator mediator)
        {
            this._scope = scope;
            this._logger = logger;
            this._con = con;
            this._mediator = mediator;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Gettest()
        {
            WebTemplate.Application.Handler.test2handler.Query data = new WebTemplate.Application.Handler.test2handler.Query();
            var rt = await _mediator.Send(data);             
            return Ok(rt);
        }

    }
}
