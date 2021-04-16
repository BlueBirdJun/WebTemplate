using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTemplate.Common.Interfaces
{
    public interface IBootStrap
    {
        IBootStrap AddDBContext(Dictionary<string, string> diccon);
        IBootStrap FinishInit();
        IBootStrap RegAutoMapper();
        IBootStrap RegMediator();
        IBootStrap RegRepsitory();
        IBootStrap RegService();
        IBootStrap SetContainerBuilder(ContainerBuilder builder, IServiceCollection services);
    }
}
