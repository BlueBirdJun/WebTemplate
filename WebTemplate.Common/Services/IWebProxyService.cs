using System.Threading.Tasks;
using WebTemplate.Common.Enums;
using WebTemplate.Common.Models;

namespace WebTemplate.Common.Services
{
    public interface IWebProxyService<T>
    {
        string CallPath { get; set; }
        HttpMethods HttpMethodValue { get; set; }
        string SendValue { get; set; }
        bool XmlYN { get; set; }

        Task<ApiEntityModel<T>> AsyncCallData();
        void Dispose();
    }
}