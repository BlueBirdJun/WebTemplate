using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebTemplate.Application.Handler;
using WebTemplate.Common.Interfaces;

namespace Business1.Api
{
    public class BootStrap : IBootStrap
    {
        
        private IServiceCollection _services;
        public IBootStrap AddDBContext(Dictionary<string, string> diccon)
        {
            return this;
        }

        public IBootStrap FinishInit()
        {
            return this;
        }

        public IBootStrap RegAutoMapper()
        {
            return this;
        }

        public IBootStrap RegMediator()
        {
            _services.AddMediatR(typeof(TestHandler.Query).Assembly);
            //_services.AddMediatR(Assembly.GetExecutingAssembly());
            return this;
        }

        public IBootStrap RegRepsitory()
        {
            return this;
        }

        public IBootStrap RegService()
        {
            return this;
        }

        public IBootStrap SetContainerBuilder( IServiceCollection services)
        {
            this._services = services;
            return this;
        }

        public IBootStrap SetContainerBuilder(ContainerBuilder builder, IServiceCollection services)
        {
            return this;
        }
    }
}
