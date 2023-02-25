using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Helper.Request;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IHttpServices
    {
        Task<T> SendPostRequest<T, U>(JsonContentPostRequest<U> request);
        Task<T> SendGetRequest<T>(GetRequest request);
    }
}
