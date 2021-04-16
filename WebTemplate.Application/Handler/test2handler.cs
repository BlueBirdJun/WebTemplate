using Autofac;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebTemplate.Common.Models;


namespace WebTemplate.Application.Handler
{
    public class test2handler
    {
        public class Query : IRequest<Result>
        {

        }

        public class Result : BaseDto
        {
            public string ReturnMessage { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ILogger<TestHandler> _logger;
            private readonly ILifetimeScope _scope;

            public Handler(ILogger<TestHandler> logger
                , ILifetimeScope scope
                )
            {
                _logger = logger;
                this._scope = scope;
            }

            public async Task<Result> Handle(Query req, CancellationToken cancellationToken)
            {
                Result dt = new Result();
                try
                {
                    dt.ReturnMessage = "cvdfdsf";
                }
                catch (Exception exc)
                {
                    dt.HasError = true;
                    dt.Message = exc.Message;
                }
                return dt;
            }

        }
    }
}
